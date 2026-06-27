using System.ComponentModel.DataAnnotations;

namespace NumberPlateWeb.Modules.Auth.ViewModels;

public class LoginRequest
{
    [Required(ErrorMessage = "Nhập tài khoản")]
    [Display(Name = "Tài khoản")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nhập mật khẩu")]
    [Display(Name = "Mật khẩu")]
    public string Password { get; set; } = string.Empty;
}
