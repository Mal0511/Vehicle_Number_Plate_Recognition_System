using System.ComponentModel.DataAnnotations;

namespace NumberPlateWeb.Modules.Scanning.ViewModels;

public class ScanRequest
{
    [Display(Name = "Manual plate fallback")]
    public string? CameraFrameReference { get; set; }

    public string? CapturedImageBase64 { get; set; }

    [Required(ErrorMessage = "Enter police officer")]
    [Display(Name = "Police officer")]
    public string OfficerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Enter location")]
    [Display(Name = "Location")]
    public string Location { get; set; } = string.Empty;
}
