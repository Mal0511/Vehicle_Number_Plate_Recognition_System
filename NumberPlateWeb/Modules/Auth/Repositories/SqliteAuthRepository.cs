using Microsoft.Data.Sqlite;
using NumberPlateWeb.Modules.Auth.Models;
using NumberPlateWeb.Modules.Database;

namespace NumberPlateWeb.Modules.Auth.Repositories;

public class SqliteAuthRepository : IAuthRepository
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public SqliteAuthRepository(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public AppUser? FindByCredentials(string username, string password)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT Username, Password, DisplayName, Role
            FROM Users
            WHERE lower(Username) = lower($username) AND Password = $password
            LIMIT 1;
            """;
        command.Parameters.AddWithValue("$username", username.Trim());
        command.Parameters.AddWithValue("$password", password);

        using var reader = command.ExecuteReader();
        return reader.Read() ? Map(reader) : null;
    }

    public IReadOnlyCollection<AppUser> GetAll()
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT Username, Password, DisplayName, Role FROM Users ORDER BY Role, Username;";

        using var reader = command.ExecuteReader();
        var users = new List<AppUser>();

        while (reader.Read())
        {
            users.Add(Map(reader));
        }

        return users;
    }

    private static AppUser Map(SqliteDataReader reader)
    {
        return new AppUser
        {
            Username = reader.GetString(0),
            Password = reader.GetString(1),
            DisplayName = reader.GetString(2),
            Role = reader.GetString(3)
        };
    }
}
