namespace NumberPlateWeb.Modules.Notifications.Models;

public class AdminNotification
{
    public int Id { get; set; }
    public string PlateNumber { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string ExternalDeliveryId { get; set; } = string.Empty;
    public string ExternalProvider { get; set; } = string.Empty;
    public bool SentSuccessfully { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
