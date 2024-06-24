using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;

public class DatabaseManager
{
    private readonly string _connectionString;

    public DatabaseManager(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task InitializeDatabaseAsync()
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        var tableCmd = @"
            CREATE TABLE IF NOT EXISTS CodingSessions (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                StartTime TEXT NOT NULL,
                EndTime TEXT NOT NULL,
                Duration TEXT NOT NULL
            )";

        await connection.ExecuteAsync(tableCmd);
    }
}
