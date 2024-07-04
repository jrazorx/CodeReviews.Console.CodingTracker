using CodingTracker.jrazorx.Models;

namespace CodingTracker.jrazorx.Repositories
{
    public interface ICodingSessionRepository
    {
        Task InitializeDatabaseAsync();
        Task InsertSessionAsync(CodingSession session);
        Task<List<CodingSession>> GetSessionsAsync(DateTime? startDate, DateTime? endDate, bool ascending = true);
        Task UpdateSessionAsync(CodingSession session);
        Task DeleteSessionAsync(int id);
    }
}