using NumberPlateWeb.Modules.VehicleLists.Models;

namespace NumberPlateWeb.Modules.VehicleLists.ViewModels;

public class VehicleListIndexViewModel
{
    public VehicleRecordInput Input { get; set; } = new();
    public IReadOnlyCollection<VehicleRecord> Vehicles { get; set; } = Array.Empty<VehicleRecord>();
}
