using NumberPlateWeb.Modules.VehicleLists.Models;

namespace NumberPlateWeb.Modules.CheckList.ViewModels;

public class CheckIndexViewModel
{
    public PlateCheckRequest Input { get; set; } = new();
    public PlateCheckResult? Result { get; set; }
    public IReadOnlyCollection<VehicleRecord> SeedData { get; set; } = Array.Empty<VehicleRecord>();
}
