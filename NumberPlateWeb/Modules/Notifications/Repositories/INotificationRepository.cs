using NumberPlateWeb.Modules.Notifications.Models;

namespace NumberPlateWeb.Modules.Notifications.Repositories;

public interface INotificationRepository
{
    IReadOnlyCollection<AdminNotification> GetAll();
    AdminNotification Add(AdminNotification notification);
}
