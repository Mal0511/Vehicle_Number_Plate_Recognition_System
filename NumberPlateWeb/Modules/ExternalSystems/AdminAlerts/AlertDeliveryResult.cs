namespace NumberPlateWeb.Modules.ExternalSystems.AdminAlerts;

public class AlertDeliveryResult
{
    public bool IsSuccess { get; set; }
    public string DeliveryId { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
    public string StatusMessage { get; set; } = string.Empty;
}
