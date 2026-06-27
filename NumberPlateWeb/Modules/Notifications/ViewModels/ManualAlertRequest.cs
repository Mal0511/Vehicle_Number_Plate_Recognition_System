using System.ComponentModel.DataAnnotations;

namespace NumberPlateWeb.Modules.Notifications.ViewModels;

public class ManualAlertRequest
{
    [Required(ErrorMessage = "Nhập biển số cần thông báo")]
    [Display(Name = "Biển số")]
    public string PlateNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nhập vị trí")]
    [Display(Name = "Vị trí")]
    public string Location { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nhập nội dung")]
    [Display(Name = "Nội dung")]
    public string Message { get; set; } = string.Empty;
}
