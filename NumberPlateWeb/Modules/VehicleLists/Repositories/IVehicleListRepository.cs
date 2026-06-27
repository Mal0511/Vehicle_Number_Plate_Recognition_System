using NumberPlateWeb.Modules.VehicleLists.Models;

namespace NumberPlateWeb.Modules.VehicleLists.Repositories;

public interface IVehicleListRepository
{
    IReadOnlyCollection<VehicleRecord> GetAll();
    VehicleRecord? Find(int id);
    VehicleRecord? FindByPlate(string normalizedPlate);
    VehicleRecord Add(VehicleRecord vehicle);
    void Remove(int id);
}
