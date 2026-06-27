using NumberPlateWeb.Modules.Shared;
using NumberPlateWeb.Modules.VehicleLists.Models;

namespace NumberPlateWeb.Modules.VehicleLists.Repositories;

public class InMemoryVehicleListRepository : IVehicleListRepository
{
    private readonly List<VehicleRecord> _vehicles =
    [
        new()
        {
            Id = 1,
            RegisteredNumber = "51A12345",
            Color = "Den",
            OwnerName = "Le Hoang Bao",
            Type = VehicleListType.Blacklist,
            Reason = "Vi pham giao thong nhieu lan",
            CreatedAt = DateTime.Now.AddDays(-3)
        },
        new()
        {
            Id = 2,
            RegisteredNumber = "30F67890",
            Color = "Trang",
            OwnerName = "Pham Thu Ha",
            Type = VehicleListType.Whitelist,
            Reason = "Xe uu tien trong khu vuc",
            CreatedAt = DateTime.Now.AddDays(-1)
        }
    ];

    private readonly object _syncRoot = new();
    private int _nextId = 3;

    public IReadOnlyCollection<VehicleRecord> GetAll()
    {
        lock (_syncRoot)
        {
            return _vehicles.Select(Clone).ToList();
        }
    }

    public VehicleRecord? Find(int id)
    {
        lock (_syncRoot)
        {
            var vehicle = _vehicles.FirstOrDefault(item => item.Id == id);
            return vehicle is null ? null : Clone(vehicle);
        }
    }

    public VehicleRecord? FindByPlate(string normalizedPlate)
    {
        lock (_syncRoot)
        {
            var vehicle = _vehicles.FirstOrDefault(item =>
                PlateNumberFormatter.Normalize(item.RegisteredNumber) == normalizedPlate);

            return vehicle is null ? null : Clone(vehicle);
        }
    }

    public VehicleRecord Add(VehicleRecord vehicle)
    {
        lock (_syncRoot)
        {
            var existing = _vehicles.FirstOrDefault(item =>
                PlateNumberFormatter.Normalize(item.RegisteredNumber) == PlateNumberFormatter.Normalize(vehicle.RegisteredNumber));

            if (existing is not null)
            {
                existing.Color = vehicle.Color;
                existing.OwnerName = vehicle.OwnerName;
                existing.Type = vehicle.Type;
                existing.Reason = vehicle.Reason;
                existing.CreatedAt = DateTime.Now;
                return Clone(existing);
            }

            vehicle.Id = _nextId++;
            _vehicles.Add(Clone(vehicle));
            return Clone(vehicle);
        }
    }

    public void Remove(int id)
    {
        lock (_syncRoot)
        {
            _vehicles.RemoveAll(item => item.Id == id);
        }
    }

    private static VehicleRecord Clone(VehicleRecord vehicle)
    {
        return new VehicleRecord
        {
            Id = vehicle.Id,
            RegisteredNumber = vehicle.RegisteredNumber,
            Color = vehicle.Color,
            OwnerName = vehicle.OwnerName,
            Type = vehicle.Type,
            Reason = vehicle.Reason,
            CreatedAt = vehicle.CreatedAt
        };
    }
}
