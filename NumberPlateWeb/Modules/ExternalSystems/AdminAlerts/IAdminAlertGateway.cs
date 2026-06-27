namespace NumberPlateWeb.Modules.ExternalSystems.AdminAlerts;

public interface IAdminAlertGateway
{
    Task<AlertDeliveryResult> SendAsync(string plateNumber, string location, string message);
}
