namespace NumberPlateWeb.Modules.PoliceManagement.Models;

public class PoliceOfficer
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string BadgeNumber { get; set; } = string.Empty;
    public string UnitName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
