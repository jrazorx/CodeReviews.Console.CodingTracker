using CodingTracker.jrazorx.Models;
using CodingTracker.jrazorx.Repositories;

namespace CodingTracker.jrazorx.Services
{
    public class CodingSessionService
    {
        private readonly ICodingSessionRepository _repository;

        public CodingSessionService(ICodingSessionRepository repository)
        {
            _repository = repository;
        }

        public async Task InitializeDatabaseAsync()
        {
            await _repository.InitializeDatabaseAsync();
        }

        public async Task InsertSessionAsync(CodingSession session)
        {
            await _repository.InsertSessionAsync(session);
        }

        public async Task<List<CodingSession>> GetSessionsAsync(DateTime? startDate, DateTime? endDate, bool ascending = true)
        {
            return await _repository.GetSessionsAsync(startDate, endDate, ascending);
        }

        public async Task<List<CodingSession>> GetSessionsByPeriodAsync(string period, bool ascending = true)
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;

            switch (period.ToLower())
            {
                case "week":
                    startDate = DateTime.Now.AddDays(-7);
                    break;
                case "month":
                    startDate = DateTime.Now.AddMonths(-1);
                    break;
                case "year":
                    startDate = DateTime.Now.AddYears(-1);
                    break;
                default:
                    return await GetSessionsAsync(null, null, ascending);
            }

            return await GetSessionsAsync(startDate, endDate, ascending);
        }

        public async Task UpdateSessionAsync(CodingSession session)
        {
            await _repository.UpdateSessionAsync(session);
        }

        public async Task DeleteSessionAsync(int id)
        {
            await _repository.DeleteSessionAsync(id);
        }

        public async Task<(TimeSpan TotalTime, TimeSpan AverageTime, int SessionCount)> GenerateReportAsync(string period)
        {
            var sessions = await GetSessionsByPeriodAsync(period);

            TimeSpan totalTime = TimeSpan.Zero;
            foreach (var session in sessions)
            {
                totalTime += session.GetDuration();
            }

            int sessionCount = sessions.Count;
            TimeSpan averageTime = sessionCount > 0 ? TimeSpan.FromTicks(totalTime.Ticks / sessionCount) : TimeSpan.Zero;

            return (totalTime, averageTime, sessionCount);
        }
    }
}