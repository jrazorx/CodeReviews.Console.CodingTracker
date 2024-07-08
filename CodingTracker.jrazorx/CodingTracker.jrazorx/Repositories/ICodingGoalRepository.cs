using CodingTracker.jrazorx.Models;

namespace CodingTracker.jrazorx.Repositories
{
    public interface ICodingGoalRepository
    {
        Task InitializeDatabaseAsync();
        Task<int> InsertGoalAsync(CodingGoal goal);
        Task<List<CodingGoal>> GetGoalsAsync();
        Task<CodingGoal> GetGoalByIdAsync(int id);
        Task UpdateGoalAsync(CodingGoal goal);
        Task DeleteGoalAsync(int id);
    }
}