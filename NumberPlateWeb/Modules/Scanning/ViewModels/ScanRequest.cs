using System.ComponentModel.DataAnnotations;

namespace NumberPlateWeb.Modules.Scanning.ViewModels;

public class ScanRequest
{
    [Required(ErrorMessage = "Nhập mã ảnh hoặc biển số mô phỏng")]
    [Display(Name = "Dữ liệu camera")]
    public string CameraFrameReference { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nhập tên cảnh sát")]
    [Display(Name = "Cảnh sát")]
    public string OfficerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nhập vị trí quét")]
    [Display(Name = "Vị trí")]
    public string Location { get; set; } = string.Empty;
}
