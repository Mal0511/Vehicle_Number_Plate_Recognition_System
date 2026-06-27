using Microsoft.Data.Sqlite;
using NumberPlateWeb.Modules.Database;
using NumberPlateWeb.Modules.Scanning.Models;

namespace NumberPlateWeb.Modules.Scanning.Repositories;

public class SqliteScanLogRepository : IScanLogRepository
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public SqliteScanLogRepository(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public IReadOnlyCollection<ScanLog> GetAll()
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT Id, PlateNumber, OfficerName, Location, RecognitionProvider, Confidence, ListStatus, NotificationSent, ScannedAt
            FROM ScanLogs
            ORDER BY ScannedAt DESC;
            """;

        using var reader = command.ExecuteReader();
        var logs = new List<ScanLog>();

        while (reader.Read())
        {
            logs.Add(Map(reader));
        }

        return logs;
    }

    public ScanLog Add(ScanLog log)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = """
            INSERT INTO ScanLogs
                (PlateNumber, OfficerName, Location, RecognitionProvider, Confidence, ListStatus, NotificationSent, ScannedAt)
            VALUES
                ($plate, $officer, $location, $provider, $confidence, $status, $notificationSent, $scannedAt)
            RETURNING Id, PlateNumber, OfficerName, Location, RecognitionProvider, Confidence, ListStatus, NotificationSent, ScannedAt;
            """;
        command.Parameters.AddWithValue("$plate", log.PlateNumber);
        command.Parameters.AddWithValue("$officer", log.OfficerName);
        command.Parameters.AddWithValue("$location", log.Location);
        command.Parameters.AddWithValue("$provider", log.RecognitionProvider);
        command.Parameters.AddWithValue("$confidence", Convert.ToDouble(log.Confidence));
        command.Parameters.AddWithValue("$status", log.ListStatus);
        command.Parameters.AddWithValue("$notificationSent", log.NotificationSent ? 1 : 0);
        command.Parameters.AddWithValue("$scannedAt", log.ScannedAt.ToString("O"));

        using var reader = command.ExecuteReader();
        return reader.Read() ? Map(reader) : log;
    }

    private static ScanLog Map(SqliteDataReader reader)
    {
        return new ScanLog
        {
            Id = reader.GetInt32(0),
            PlateNumber = reader.GetString(1),
            OfficerName = reader.GetString(2),
            Location = reader.GetString(3),
            RecognitionProvider = reader.GetString(4),
            Confidence = Convert.ToDecimal(reader.GetDouble(5)),
            ListStatus = reader.GetString(6),
            NotificationSent = reader.GetInt32(7) == 1,
            ScannedAt = DateTime.Parse(reader.GetString(8))
        };
    }
}
