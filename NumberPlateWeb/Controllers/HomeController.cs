using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NumberPlateWeb.Models;
using NumberPlateWeb.Modules.Auth.Services;
using NumberPlateWeb.Modules.Dashboard.ViewModels;
using NumberPlateWeb.Modules.Notifications.Services;
using NumberPlateWeb.Modules.Scanning.Services;
using NumberPlateWeb.Modules.VehicleLists.Services;

namespace NumberPlateWeb.Controllers;

public class HomeController : Controller
{
    private readonly AuthService _authService;
    private readonly VehicleListService _vehicleListService;
    private readonly ScanService _scanService;
    private readonly NotificationService _notificationService;

    public HomeController(
        AuthService authService,
        VehicleListService vehicleListService,
        ScanService scanService,
        NotificationService notificationService)
    {
        _authService = authService;
        _vehicleListService = vehicleListService;
        _scanService = scanService;
        _notificationService = notificationService;
    }

    public IActionResult Index()
    {
        var scans = _scanService.GetRecentLogs();
        var notifications = _notificationService.GetAll();

        ViewBag.DisplayName = HttpContext.Session.GetString("DisplayName") ?? "Chưa đăng nhập";
        ViewBag.Role = HttpContext.Session.GetString("Role") ?? "-";

        return View(new DashboardViewModel
        {
            PoliceCount = _authService.UserCount(),
            VehicleCount = _vehicleListService.GetAll().Count,
            ScanCount = scans.Count,
            NotificationCount = notifications.Count,
            RecentScans = scans.Take(5).ToList(),
            RecentNotifications = notifications.Take(5).ToList()
        });
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
