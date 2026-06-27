using NumberPlateWeb.Modules.Notifications.Models;

namespace NumberPlateWeb.Modules.Notifications.ViewModels;

public class NotificationsIndexViewModel
{
    public ManualAlertRequest Input { get; set; } = new();
    public IReadOnlyCollection<AdminNotification> Notifications { get; set; } = Array.Empty<AdminNotification>();
}
