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
                    EndTime TEXT NOT NULL,
                    Duration TEXT NOT NULL
                )");
        }

        public async Task InsertSessionAsync(CodingSession session)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.ExecuteAsync(@"
                INSERT INTO CodingSessions (StartTime, EndTime, Duration)
                VALUES (@StartTime, @EndTime, @Duration)",
                new {
                    session.StartTime,
                    session.EndTime,
                    Duration = session.Duration.ToString()
                });
        }
    }
}