using System.Configuration;
using Dapper;
using Microsoft.Data.Sqlite;

namespace CodingTracker
{
    public class DatabaseManager
    {
        private readonly string _connectionString;

        public DatabaseManager()
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

        public async Task<List<CodingSession>> GetAllSessionsAsync()
        {
            using var connection = new SqliteConnection(_connectionString);
            var sessions = await connection.QueryAsync<CodingSession>(@"
                 SELECT Id,
                        StartTime,
                        EndTime
                   FROM CodingSessions
            ");
            return sessions.AsList();
        }
    }
}