using NumberPlateWeb.Modules.Shared;

namespace NumberPlateWeb.Modules.VehicleLists.Models;

public class VehicleRecord
{
    public int Id { get; set; }
    public string RegisteredNumber { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string OwnerName { get; set; } = string.Empty;
    public VehicleListType Type { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
