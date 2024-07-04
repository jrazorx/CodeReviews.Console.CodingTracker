using CodingTracker.jrazorx.Models;

namespace CodingTracker.jrazorx.Repositories
{
    public interface ICodingSessionRepository
    {
        Task InitializeDatabaseAsync();
        Task InsertSessionAsync(CodingSession session);
        Task<List<CodingSession>> GetAllSessionsAsync();
        Task UpdateSessionAsync(CodingSession session);
        Task DeleteSessionAsync(int id);
    }
}