using NumberPlateWeb.Modules.Shared;
using NumberPlateWeb.Modules.VehicleLists.Models;
using NumberPlateWeb.Modules.VehicleLists.Repositories;
using NumberPlateWeb.Modules.VehicleLists.ViewModels;

namespace NumberPlateWeb.Modules.VehicleLists.Services;

public class VehicleListService
{
    private readonly IVehicleListRepository _repository;

    public VehicleListService(IVehicleListRepository repository)
    {
        _repository = repository;
    }

    public IReadOnlyCollection<VehicleRecord> GetAll()
    {
        return _repository.GetAll()
            .OrderBy(record => record.Type)
            .ThenBy(record => record.RegisteredNumber)
            .ToList();
    }

    public VehicleRecord? FindByPlate(string plate)
    {
        return _repository.FindByPlate(PlateNumberFormatter.Normalize(plate));
    }

    public VehicleRecord Save(VehicleRecordInput input)
    {
        var vehicle = new VehicleRecord
        {
            RegisteredNumber = PlateNumberFormatter.Normalize(input.RegisteredNumber),
            Color = input.Color.Trim(),
            OwnerName = input.OwnerName.Trim(),
            Type = input.Type,
            Reason = input.Reason.Trim(),
            CreatedAt = DateTime.Now
        };

        return _repository.Add(vehicle);
    }

    public void Delete(int id)
    {
        _repository.Remove(id);
    }
}
