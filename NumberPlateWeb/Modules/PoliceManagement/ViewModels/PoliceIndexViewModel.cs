using NumberPlateWeb.Modules.PoliceManagement.Models;

namespace NumberPlateWeb.Modules.PoliceManagement.ViewModels;

public class PoliceIndexViewModel
{
    public PoliceOfficerInput Input { get; set; } = new();
    public IReadOnlyCollection<PoliceOfficer> Officers { get; set; } = Array.Empty<PoliceOfficer>();
}
