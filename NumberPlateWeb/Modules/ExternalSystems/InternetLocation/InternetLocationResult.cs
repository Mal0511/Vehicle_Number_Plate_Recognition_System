namespace NumberPlateWeb.Modules.ExternalSystems.InternetLocation;

public class InternetLocationResult
{
    public string Location { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
    public bool InternetAvailable { get; set; }
}
