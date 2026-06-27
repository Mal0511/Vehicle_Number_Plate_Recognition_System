using NumberPlateWeb.Modules.ExternalSystems.AdminAlerts;
using NumberPlateWeb.Modules.Notifications.Models;
using NumberPlateWeb.Modules.Notifications.Repositories;
using NumberPlateWeb.Modules.Notifications.ViewModels;
using NumberPlateWeb.Modules.Shared;
using NumberPlateWeb.Modules.VehicleLists.Models;

namespace NumberPlateWeb.Modules.Notifications.Services;

public class NotificationService
{
    private readonly INotificationRepository _repository;
    private readonly IAdminAlertGateway _alertGateway;

    public NotificationService(INotificationRepository repository, IAdminAlertGateway alertGateway)
    {
        _repository = repository;
        _alertGateway = alertGateway;
    }

    public IReadOnlyCollection<AdminNotification> GetAll()
    {
        return _repository.GetAll()
            .OrderByDescending(notification => notification.CreatedAt)
            .ToList();
    }

    public async Task<AdminNotification> NotifyBlacklistedVehicleAsync(VehicleRecord vehicle, string location)
    {
        var message = $"Blacklisted vehicle {PlateNumberFormatter.Display(vehicle.RegisteredNumber)} spotted. Reason: {vehicle.Reason}";
        var delivery = await _alertGateway.SendAsync(vehicle.RegisteredNumber, location, message);

        return _repository.Add(new AdminNotification
        {
            PlateNumber = vehicle.RegisteredNumber,
            Location = location,
            Message = message,
            ExternalDeliveryId = delivery.DeliveryId,
            ExternalProvider = delivery.Provider,
            SentSuccessfully = delivery.IsSuccess,
            CreatedAt = DateTime.Now
        });
    }

    public async Task<AdminNotification> SendManualAlertAsync(ManualAlertRequest input)
    {
        var plateNumber = PlateNumberFormatter.Normalize(input.PlateNumber);
        var delivery = await _alertGateway.SendAsync(plateNumber, input.Location.Trim(), input.Message.Trim());

        return _repository.Add(new AdminNotification
        {
            PlateNumber = plateNumber,
            Location = input.Location.Trim(),
            Message = input.Message.Trim(),
            ExternalDeliveryId = delivery.DeliveryId,
            ExternalProvider = delivery.Provider,
            SentSuccessfully = delivery.IsSuccess,
            CreatedAt = DateTime.Now
        });
    }
}
