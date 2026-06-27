namespace NumberPlateWeb.Modules.Scanning.Models;

public class ScanLog
{
    public int Id { get; set; }
    public string PlateNumber { get; set; } = string.Empty;
    public string OfficerName { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string RecognitionProvider { get; set; } = string.Empty;
    public decimal Confidence { get; set; }
    public string ListStatus { get; set; } = "Not Found";
    public bool NotificationSent { get; set; }
    public DateTime ScannedAt { get; set; } = DateTime.Now;
}
