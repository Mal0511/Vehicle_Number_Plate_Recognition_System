namespace NumberPlateWeb.Modules.CheckList.ViewModels;

public class PlateCheckResult
{
    public string PlateNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string OwnerName { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public bool Found { get; set; }
}
