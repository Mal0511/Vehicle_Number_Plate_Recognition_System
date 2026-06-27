using NumberPlateWeb.Modules.Scanning.Models;

namespace NumberPlateWeb.Modules.Scanning.ViewModels;

public class ScanIndexViewModel
{
    public ScanRequest Input { get; set; } = new();
    public ScanResultViewModel? Result { get; set; }
    public IReadOnlyCollection<ScanLog> Logs { get; set; } = Array.Empty<ScanLog>();
}
