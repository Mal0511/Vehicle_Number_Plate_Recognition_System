using Microsoft.Data.Sqlite;
using NumberPlateWeb.Modules.Database;
using NumberPlateWeb.Modules.Shared;
using NumberPlateWeb.Modules.VehicleLists.Models;

namespace NumberPlateWeb.Modules.VehicleLists.Repositories;

public class SqliteVehicleListRepository : IVehicleListRepository
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public SqliteVehicleListRepository(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public IReadOnlyCollection<VehicleRecord> GetAll()
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT Id, RegisteredNumber, Color, OwnerName, Type, Reason, CreatedAt
            FROM VehicleRecords
            ORDER BY Type, RegisteredNumber;
            """;

        using var reader = command.ExecuteReader();
        var records = new List<VehicleRecord>();

        while (reader.Read())
        {
            records.Add(Map(reader));
        }

        return records;
    }

    public VehicleRecord? Find(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT Id, RegisteredNumber, Color, OwnerName, Type, Reason, CreatedAt
            FROM VehicleRecords
            WHERE Id = $id
            LIMIT 1;
            """;
        command.Parameters.AddWithValue("$id", id);

        using var reader = command.ExecuteReader();
        return reader.Read() ? Map(reader) : null;
    }

    public VehicleRecord? FindByPlate(string normalizedPlate)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT Id, RegisteredNumber, Color, OwnerName, Type, Reason, CreatedAt
            FROM VehicleRecords
            WHERE RegisteredNumber = $plate
            LIMIT 1;
            """;
        command.Parameters.AddWithValue("$plate", PlateNumberFormatter.Normalize(normalizedPlate));

        using var reader = command.ExecuteReader();
        return reader.Read() ? Map(reader) : null;
    }

    public VehicleRecord Add(VehicleRecord vehicle)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = """
            INSERT INTO VehicleRecords (RegisteredNumber, Color, OwnerName, Type, Reason, CreatedAt)
            VALUES ($plate, $color, $owner, $type, $reason, $createdAt)
            ON CONFLICT(RegisteredNumber) DO UPDATE SET
                Color = excluded.Color,
                OwnerName = excluded.OwnerName,
                Type = excluded.Type,
                Reason = excluded.Reason,
                CreatedAt = excluded.CreatedAt
            RETURNING Id, RegisteredNumber, Color, OwnerName, Type, Reason, CreatedAt;
            """;
        command.Parameters.AddWithValue("$plate", PlateNumberFormatter.Normalize(vehicle.RegisteredNumber));
        command.Parameters.AddWithValue("$color", vehicle.Color);
        command.Parameters.AddWithValue("$owner", vehicle.OwnerName);
        command.Parameters.AddWithValue("$type", (int)vehicle.Type);
        command.Parameters.AddWithValue("$reason", vehicle.Reason);
        command.Parameters.AddWithValue("$createdAt", DateTime.Now.ToString("O"));

        using var reader = command.ExecuteReader();
        return reader.Read() ? Map(reader) : vehicle;
    }

    public void Remove(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM VehicleRecords WHERE Id = $id;";
        command.Parameters.AddWithValue("$id", id);
        command.ExecuteNonQuery();
    }

    private static VehicleRecord Map(SqliteDataReader reader)
    {
        return new VehicleRecord
        {
            Id = reader.GetInt32(0),
            RegisteredNumber = reader.GetString(1),
            Color = reader.GetString(2),
            OwnerName = reader.GetString(3),
            Type = (VehicleListType)reader.GetInt32(4),
            Reason = reader.GetString(5),
            CreatedAt = DateTime.Parse(reader.GetString(6))
        };
    }
}
