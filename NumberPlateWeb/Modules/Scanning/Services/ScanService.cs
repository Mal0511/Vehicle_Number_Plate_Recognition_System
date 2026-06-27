using NumberPlateWeb.Modules.ExternalSystems.InternetLocation;
using NumberPlateWeb.Modules.ExternalSystems.PlateRecognition;
using NumberPlateWeb.Modules.Notifications.Services;
using NumberPlateWeb.Modules.Scanning.Models;
using NumberPlateWeb.Modules.Scanning.Repositories;
using NumberPlateWeb.Modules.Scanning.ViewModels;
using NumberPlateWeb.Modules.Shared;
using NumberPlateWeb.Modules.VehicleLists.Services;

namespace NumberPlateWeb.Modules.Scanning.Services;

public class ScanService
{
    private readonly IScanLogRepository _scanLogRepository;
    private readonly IPlateRecognitionGateway _plateRecognitionGateway;
    private readonly IInternetLocationService _internetLocationService;
    private readonly VehicleListService _vehicleListService;
    private readonly NotificationService _notificationService;

    public ScanService(
        IScanLogRepository scanLogRepository,
        IPlateRecognitionGateway plateRecognitionGateway,
        IInternetLocationService internetLocationService,
        VehicleListService vehicleListService,
        NotificationService notificationService)
    {
        _scanLogRepository = scanLogRepository;
        _plateRecognitionGateway = plateRecognitionGateway;
        _internetLocationService = internetLocationService;
        _vehicleListService = vehicleListService;
        _notificationService = notificationService;
    }

    public IReadOnlyCollection<ScanLog> GetRecentLogs()
    {
        return _scanLogRepository.GetAll()
            .OrderByDescending(log => log.ScannedAt)
            .ToList();
    }

    public async Task<ScanResultViewModel> ScanAsync(ScanRequest request)
    {
        var recognition = await _plateRecognitionGateway.RecognizeAsync(
            request.CameraFrameReference ?? string.Empty,
            request.CapturedImageBase64);
        var location = await _internetLocationService.ResolveLocationAsync(request.Location);
        var vehicle = _vehicleListService.FindByPlate(recognition.PlateNumber);
        var listStatus = vehicle is null ? "Not Found" : vehicle.Type.ToDisplayName();
        var notificationSent = false;
        var alertStatus = "No alert required";

        if (vehicle?.Type == VehicleListType.Blacklist)
        {
            var notification = await _notificationService.NotifyBlacklistedVehicleAsync(vehicle, location.Location);
            notificationSent = notification.SentSuccessfully;
            alertStatus = notification.SentSuccessfully
                ? $"Alert sent: {notification.ExternalDeliveryId}"
                : "Alert failed";
        }

        var log = _scanLogRepository.Add(new ScanLog
        {
            PlateNumber = recognition.PlateNumber,
            OfficerName = request.OfficerName.Trim(),
            Location = location.Location,
            RecognitionProvider = recognition.Provider,
            Confidence = recognition.Confidence,
            ListStatus = listStatus,
            NotificationSent = notificationSent,
            ScannedAt = DateTime.Now
        });

        return new ScanResultViewModel
        {
            PlateNumber = recognition.PlateNumber,
            Confidence = recognition.Confidence,
            RecognitionProvider = recognition.Provider,
            LocationProvider = location.Provider,
            RawOcrText = recognition.RawPayload,
            ListStatus = listStatus,
            AlertStatus = alertStatus,
            ScanLog = log
        };
    }
}
