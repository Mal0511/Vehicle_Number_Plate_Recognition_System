using System.ComponentModel.DataAnnotations;

namespace NumberPlateWeb.Modules.PoliceManagement.ViewModels;

public class PoliceOfficerInput
{
    [Required(ErrorMessage = "Nhập họ tên cảnh sát")]
    [Display(Name = "Họ tên")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nhập mã cảnh sát")]
    [Display(Name = "Mã cảnh sát")]
    public string BadgeNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nhập đơn vị")]
    [Display(Name = "Đơn vị")]
    public string UnitName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nhập số điện thoại")]
    [Display(Name = "Số điện thoại")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nhập tài khoản đăng nhập")]
    [Display(Name = "Tài khoản")]
    public string Username { get; set; } = string.Empty;
}
