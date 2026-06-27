using System.ComponentModel.DataAnnotations;

namespace NumberPlateWeb.Modules.CheckList.ViewModels;

public class PlateCheckRequest
{
    [Required(ErrorMessage = "Nhập biển số cần kiểm tra")]
    [Display(Name = "Biển số")]
    public string PlateNumber { get; set; } = string.Empty;
}
