using Microsoft.Data.Sqlite;

namespace NumberPlateWeb.Modules.Database;

public class SqliteConnectionFactory
{
    private readonly string _connectionString;

    public SqliteConnectionFactory(IWebHostEnvironment environment)
    {
        var appDataPath = Path.Combine(environment.ContentRootPath, "App_Data");
        Directory.CreateDirectory(appDataPath);

        DatabasePath = Path.Combine(appDataPath, "numberplate.db");
        _connectionString = new SqliteConnectionStringBuilder
        {
            DataSource = DatabasePath
        }.ToString();
    }

    public string DatabasePath { get; }

    public SqliteConnection CreateConnection()
    {
        return new SqliteConnection(_connectionString);
    }
}
