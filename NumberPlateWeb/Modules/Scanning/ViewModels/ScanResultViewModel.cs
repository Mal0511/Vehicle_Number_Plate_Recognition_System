using NumberPlateWeb.Modules.Scanning.Models;

namespace NumberPlateWeb.Modules.Scanning.ViewModels;

public class ScanResultViewModel
{
    public string PlateNumber { get; set; } = string.Empty;
    public decimal Confidence { get; set; }
    public string RecognitionProvider { get; set; } = string.Empty;
    public string LocationProvider { get; set; } = string.Empty;
    public string ListStatus { get; set; } = string.Empty;
    public string AlertStatus { get; set; } = string.Empty;
    public ScanLog? ScanLog { get; set; }
}
