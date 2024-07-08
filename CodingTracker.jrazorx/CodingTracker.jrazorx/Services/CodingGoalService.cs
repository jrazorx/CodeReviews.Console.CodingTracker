using CodingTracker.jrazorx.Models;
using CodingTracker.jrazorx.Repositories;

namespace CodingTracker.jrazorx.Services
{
    public class CodingGoalService
    {
        private readonly ICodingGoalRepository _repository;
        private readonly CodingSessionService _sessionService;

        public CodingGoalService(ICodingGoalRepository repository, CodingSessionService sessionService)
        {
            _repository = repository;
            _sessionService = sessionService;
        }

        public async Task InitializeDatabaseAsync()
        {
            await _repository.InitializeDatabaseAsync();
        }

        public async Task<int> AddGoalAsync(CodingGoal goal)
        {
            return await _repository.InsertGoalAsync(goal);
        }

        public async Task<List<CodingGoal>> GetGoalsAsync()
        {
            return await _repository.GetGoalsAsync();
        }

        public async Task<CodingGoal> GetGoalByIdAsync(int id)
        {
            return await _repository.GetGoalByIdAsync(id);
        }

        public async Task UpdateGoalAsync(CodingGoal goal)
        {
            await _repository.UpdateGoalAsync(goal);
        }

        public async Task DeleteGoalAsync(int id)
        {
            await _repository.DeleteGoalAsync(id);
        }

        public async Task<double> GetHoursPerDayForGoalAsync(CodingGoal goal)
        {
            var sessions = await _sessionService.GetSessionsAsync(goal.StartDate, goal.EndDate);
            int totalHoursCompleted = (int)sessions.Sum(s => s.GetDuration().TotalHours);
            return goal.GetHoursPerDay(totalHoursCompleted);
        }

        public async Task<double> GetTotalHoursSpentForGoalAsync(CodingGoal goal)
        {
            var sessions = await _sessionService.GetSessionsAsync(goal.StartDate, goal.EndDate);
            return sessions.Sum(s => (s.EndTime - s.StartTime).TotalHours);
        }
    }
}