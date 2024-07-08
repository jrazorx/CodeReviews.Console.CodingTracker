using System.Configuration;
using CodingTracker.jrazorx.Models;
using Dapper;
using Microsoft.Data.Sqlite;

namespace CodingTracker.jrazorx.Repositories
{
    public class CodingGoalRepository : ICodingGoalRepository
    {
        private readonly string _connectionString;

        public CodingGoalRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["CodingTrackerDB"].ConnectionString;
        }

        public async Task InitializeDatabaseAsync()
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            await connection.ExecuteAsync(@"
                CREATE TABLE IF NOT EXISTS CodingGoals (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    StartDate TEXT NOT NULL,
                    EndDate TEXT NOT NULL,
                    TargetHours INTEGER NOT NULL
                )");
        }

        public async Task<int> InsertGoalAsync(CodingGoal goal)
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = @"
                INSERT INTO CodingGoals (StartDate, EndDate, TargetHours)
                VALUES (@StartDate, @EndDate, @TargetHours);
                SELECT last_insert_rowid();";

            return await connection.ExecuteScalarAsync<int>(sql, goal);
        }

        public async Task<List<CodingGoal>> GetGoalsAsync()
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = "SELECT * FROM CodingGoals ORDER BY StartDate";
            var goals = await connection.QueryAsync<CodingGoal>(sql);
            return goals.ToList();
        }

        public async Task<CodingGoal> GetGoalByIdAsync(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = "SELECT * FROM CodingGoals WHERE Id = @Id";
            return await connection.QuerySingleOrDefaultAsync<CodingGoal>(sql, new { Id = id });
        }

        public async Task UpdateGoalAsync(CodingGoal goal)
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = @"
                UPDATE CodingGoals 
                SET StartDate = @StartDate, 
                    EndDate = @EndDate, 
                    TargetHours = @TargetHours 
                WHERE Id = @Id";

            await connection.ExecuteAsync(sql, goal);
        }

        public async Task DeleteGoalAsync(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = "DELETE FROM CodingGoals WHERE Id = @Id";
            await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}