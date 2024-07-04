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

        public async Task<List<CodingSession>> GetAllSessionsAsync()
        {
            return await _repository.GetAllSessionsAsync();
        }

        public async Task UpdateSessionAsync(CodingSession session)
        {
            await _repository.UpdateSessionAsync(session);
        }

        public async Task DeleteSessionAsync(int id)
        {
            await _repository.DeleteSessionAsync(id);
        }
    }
}