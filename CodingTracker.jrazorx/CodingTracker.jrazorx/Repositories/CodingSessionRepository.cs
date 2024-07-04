using System.Configuration;
using CodingTracker.jrazorx.Models;
using Dapper;
using Microsoft.Data.Sqlite;

namespace CodingTracker.jrazorx.Repositories
{
    public class CodingSessionRepository : ICodingSessionRepository
    {
        private readonly string _connectionString;

        public CodingSessionRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["CodingTrackerDB"].ConnectionString;
        }

        public async Task InitializeDatabaseAsync()
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            await connection.ExecuteAsync(@"
                CREATE TABLE IF NOT EXISTS CodingSessions (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    StartTime TEXT NOT NULL,
                    EndTime TEXT NOT NULL
                )");
        }

        public async Task InsertSessionAsync(CodingSession session)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.ExecuteAsync(@"
                INSERT INTO CodingSessions (
                    StartTime,
                    EndTime)
                VALUES (
                    @StartTime,
                    @EndTime
                )",
                new { session.StartTime, session.EndTime });
        }

        public async Task<List<CodingSession>> GetSessionsAsync(DateTime? startDate, DateTime? endDate, bool ascending = true)
        {
            using var connection = new SqliteConnection(_connectionString);
            var query = @"
               SELECT Id, StartTime, EndTime
                 FROM CodingSessions
                WHERE (@StartDate IS NULL
                   OR  StartTime >= @StartDate)
                  AND (@EndDate IS NULL
                   OR  EndTime <= @EndDate)
               ORDER BY Id " + (ascending ? "ASC" : "DESC");

            var sessions = await connection.QueryAsync<CodingSession>(query, new { StartDate = startDate, EndDate = endDate });
            return sessions.AsList();
        }

        public async Task UpdateSessionAsync(CodingSession session)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.ExecuteAsync(@"
                 UPDATE CodingSessions
                    SET StartTime = @StartTime,
                        EndTime = @EndTime
                  WHERE Id = @Id",
                        new { session.StartTime, session.EndTime, session.Id });
        }

        public async Task DeleteSessionAsync(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.ExecuteAsync(@"
                 DELETE FROM CodingSessions
                  WHERE Id = @Id",
                        new { Id = id });
        }
    }
}