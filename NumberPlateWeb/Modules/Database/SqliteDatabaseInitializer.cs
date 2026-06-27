using Microsoft.Data.Sqlite;

namespace NumberPlateWeb.Modules.Database;

public class SqliteDatabaseInitializer
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public SqliteDatabaseInitializer(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public void Initialize()
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        ExecuteNonQuery(connection, """
            CREATE TABLE IF NOT EXISTS Users (
                Username TEXT PRIMARY KEY,
                Password TEXT NOT NULL,
                DisplayName TEXT NOT NULL,
                Role TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS VehicleRecords (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                RegisteredNumber TEXT NOT NULL UNIQUE,
                Color TEXT NOT NULL,
                OwnerName TEXT NOT NULL,
                Type INTEGER NOT NULL,
                Reason TEXT NOT NULL,
                CreatedAt TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS ScanLogs (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                PlateNumber TEXT NOT NULL,
                OfficerName TEXT NOT NULL,
                Location TEXT NOT NULL,
                RecognitionProvider TEXT NOT NULL,
                Confidence REAL NOT NULL,
                ListStatus TEXT NOT NULL,
                NotificationSent INTEGER NOT NULL,
                ScannedAt TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Notifications (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                PlateNumber TEXT NOT NULL,
                Location TEXT NOT NULL,
                Message TEXT NOT NULL,
                ExternalDeliveryId TEXT NOT NULL,
                ExternalProvider TEXT NOT NULL,
                SentSuccessfully INTEGER NOT NULL,
                CreatedAt TEXT NOT NULL
            );
            """);

        SeedUsers(connection);
        SeedVehicleRecords(connection);
        SeedScanLogs(connection);
        SeedNotifications(connection);
    }

    private static void SeedUsers(SqliteConnection connection)
    {
        ExecuteNonQuery(connection, """
            INSERT OR IGNORE INTO Users (Username, Password, DisplayName, Role)
            VALUES
                ('admin', 'admin123', 'System Admin', 'Admin'),
                ('police', 'police123', 'Police Officer', 'Police');
            """);
    }

    private static void SeedVehicleRecords(SqliteConnection connection)
    {
        ExecuteNonQuery(connection, """
            INSERT OR IGNORE INTO VehicleRecords (RegisteredNumber, Color, OwnerName, Type, Reason, CreatedAt)
            VALUES
                ('51A12345', 'Den', 'Le Hoang Bao', 1, 'Vi pham giao thong nhieu lan', '2026-06-20T08:00:00'),
                ('30F67890', 'Trang', 'Pham Thu Ha', 2, 'Xe uu tien trong khu vuc', '2026-06-21T09:30:00'),
                ('59C24680', 'Do', 'Nguyen Duc Minh', 1, 'Chua nop phat nguoi', '2026-06-22T10:45:00'),
                ('43B13579', 'Xanh', 'Tran Bao Ngoc', 2, 'Xe da xac minh hop le', '2026-06-23T11:15:00');
            """);
    }

    private static void SeedScanLogs(SqliteConnection connection)
    {
        if (CountRows(connection, "ScanLogs") > 0)
        {
            return;
        }

        ExecuteNonQuery(connection, """
            INSERT INTO ScanLogs
                (PlateNumber, OfficerName, Location, RecognitionProvider, Confidence, ListStatus, NotificationSent, ScannedAt)
            VALUES
                ('51A12345', 'Police Officer', 'Gate A', 'External OCR System', 0.96, 'Blacklist', 1, '2026-06-24T08:20:00'),
                ('30F67890', 'Police Officer', 'Gate B', 'External OCR System', 0.94, 'Whitelist', 0, '2026-06-24T09:10:00');
            """);
    }

    private static void SeedNotifications(SqliteConnection connection)
    {
        if (CountRows(connection, "Notifications") > 0)
        {
            return;
        }

        ExecuteNonQuery(connection, """
            INSERT INTO Notifications
                (PlateNumber, Location, Message, ExternalDeliveryId, ExternalProvider, SentSuccessfully, CreatedAt)
            VALUES
                ('51A12345', 'Gate A', 'Blacklisted vehicle 51A-12345 spotted. Reason: Vi pham giao thong nhieu lan', 'ADM-SEED-001', 'Internet / Location Service', 1, '2026-06-24T08:20:05');
            """);
    }

    private static long CountRows(SqliteConnection connection, string tableName)
    {
        using var command = connection.CreateCommand();
        command.CommandText = $"SELECT COUNT(*) FROM {tableName};";
        return (long)(command.ExecuteScalar() ?? 0L);
    }

    private static void ExecuteNonQuery(SqliteConnection connection, string sql)
    {
        using var command = connection.CreateCommand();
        command.CommandText = sql;
        command.ExecuteNonQuery();
    }
}
