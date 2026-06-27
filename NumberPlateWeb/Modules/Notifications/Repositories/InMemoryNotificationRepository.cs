using NumberPlateWeb.Modules.Notifications.Models;

namespace NumberPlateWeb.Modules.Notifications.Repositories;

public class InMemoryNotificationRepository : INotificationRepository
{
    private readonly List<AdminNotification> _notifications = [];
    private readonly object _syncRoot = new();
    private int _nextId = 1;

    public IReadOnlyCollection<AdminNotification> GetAll()
    {
        lock (_syncRoot)
        {
            return _notifications.Select(Clone).ToList();
        }
    }

    public AdminNotification Add(AdminNotification notification)
    {
        lock (_syncRoot)
        {
            notification.Id = _nextId++;
            _notifications.Add(Clone(notification));
            return Clone(notification);
        }
    }

    private static AdminNotification Clone(AdminNotification notification)
    {
        return new AdminNotification
        {
            Id = notification.Id,
            PlateNumber = notification.PlateNumber,
            Location = notification.Location,
            Message = notification.Message,
            ExternalDeliveryId = notification.ExternalDeliveryId,
            ExternalProvider = notification.ExternalProvider,
            SentSuccessfully = notification.SentSuccessfully,
            CreatedAt = notification.CreatedAt
        };
    }
}
