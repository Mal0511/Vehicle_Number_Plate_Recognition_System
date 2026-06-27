using NumberPlateWeb.Modules.Notifications.Models;
using NumberPlateWeb.Modules.Scanning.Models;

namespace NumberPlateWeb.Modules.Dashboard.ViewModels;

public class DashboardViewModel
{
    public int PoliceCount { get; set; }
    public int VehicleCount { get; set; }
    public int ScanCount { get; set; }
    public int NotificationCount { get; set; }
    public IReadOnlyCollection<ScanLog> RecentScans { get; set; } = Array.Empty<ScanLog>();
    public IReadOnlyCollection<AdminNotification> RecentNotifications { get; set; } = Array.Empty<AdminNotification>();
}
