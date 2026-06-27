namespace NumberPlateWeb.Modules.ExternalSystems.AdminAlerts;

public class MockAdminAlertGateway : IAdminAlertGateway
{
    public async Task<AlertDeliveryResult> SendAsync(string plateNumber, string location, string message)
    {
        await Task.Delay(160);

        return new AlertDeliveryResult
        {
            IsSuccess = true,
            DeliveryId = $"ADM-{DateTime.Now:yyyyMMddHHmmssfff}",
            Provider = "External Admin Notification Gateway",
            StatusMessage = $"Sent alert for {plateNumber} at {location}"
        };
    }
}
