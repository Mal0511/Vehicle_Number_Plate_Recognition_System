using System.ComponentModel.DataAnnotations;
using NumberPlateWeb.Modules.Shared;

namespace NumberPlateWeb.Modules.VehicleLists.ViewModels;

public class VehicleRecordInput
{
    [Required(ErrorMessage = "Nhập biển số xe")]
    [Display(Name = "Biển số")]
    public string RegisteredNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nhập màu xe")]
    [Display(Name = "Màu xe")]
    public string Color { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nhập chủ xe")]
    [Display(Name = "Chủ xe")]
    public string OwnerName { get; set; } = string.Empty;

    [Display(Name = "Loại danh sách")]
    public VehicleListType Type { get; set; } = VehicleListType.Blacklist;

    [Required(ErrorMessage = "Nhập lý do")]
    [Display(Name = "Lý do")]
    public string Reason { get; set; } = string.Empty;
}
