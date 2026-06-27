using Microsoft.Data.Sqlite;
using NumberPlateWeb.Modules.Database;
using NumberPlateWeb.Modules.Notifications.Models;

namespace NumberPlateWeb.Modules.Notifications.Repositories;

public class SqliteNotificationRepository : INotificationRepository
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public SqliteNotificationRepository(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public IReadOnlyCollection<AdminNotification> GetAll()
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT Id, PlateNumber, Location, Message, ExternalDeliveryId, ExternalProvider, SentSuccessfully, CreatedAt
            FROM Notifications
            ORDER BY CreatedAt DESC;
            """;

        using var reader = command.ExecuteReader();
        var notifications = new List<AdminNotification>();

        while (reader.Read())
        {
            notifications.Add(Map(reader));
        }

        return notifications;
    }

    public AdminNotification Add(AdminNotification notification)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = """
            INSERT INTO Notifications
                (PlateNumber, Location, Message, ExternalDeliveryId, ExternalProvider, SentSuccessfully, CreatedAt)
            VALUES
                ($plate, $location, $message, $deliveryId, $provider, $sent, $createdAt)
            RETURNING Id, PlateNumber, Location, Message, ExternalDeliveryId, ExternalProvider, SentSuccessfully, CreatedAt;
            """;
        command.Parameters.AddWithValue("$plate", notification.PlateNumber);
        command.Parameters.AddWithValue("$location", notification.Location);
        command.Parameters.AddWithValue("$message", notification.Message);
        command.Parameters.AddWithValue("$deliveryId", notification.ExternalDeliveryId);
        command.Parameters.AddWithValue("$provider", notification.ExternalProvider);
        command.Parameters.AddWithValue("$sent", notification.SentSuccessfully ? 1 : 0);
        command.Parameters.AddWithValue("$createdAt", notification.CreatedAt.ToString("O"));

        using var reader = command.ExecuteReader();
        return reader.Read() ? Map(reader) : notification;
    }

    private static AdminNotification Map(SqliteDataReader reader)
    {
        return new AdminNotification
        {
            Id = reader.GetInt32(0),
            PlateNumber = reader.GetString(1),
            Location = reader.GetString(2),
            Message = reader.GetString(3),
            ExternalDeliveryId = reader.GetString(4),
            ExternalProvider = reader.GetString(5),
            SentSuccessfully = reader.GetInt32(6) == 1,
            CreatedAt = DateTime.Parse(reader.GetString(7))
        };
    }
}
