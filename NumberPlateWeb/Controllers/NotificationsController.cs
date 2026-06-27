using Microsoft.AspNetCore.Mvc;
using NumberPlateWeb.Modules.Notifications.Services;
using NumberPlateWeb.Modules.Notifications.ViewModels;

namespace NumberPlateWeb.Controllers;

public class NotificationsController : Controller
{
    private readonly NotificationService _notificationService;

    public NotificationsController(NotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public IActionResult Index()
    {
        return View(BuildViewModel(new ManualAlertRequest()));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Send([Bind(Prefix = "Input")] ManualAlertRequest input)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", BuildViewModel(input));
        }

        await _notificationService.SendManualAlertAsync(input);
        TempData["SuccessMessage"] = "Đã gửi thông báo qua external gateway.";
        return RedirectToAction(nameof(Index));
    }

    private NotificationsIndexViewModel BuildViewModel(ManualAlertRequest input)
    {
        return new NotificationsIndexViewModel
        {
            Input = input,
            Notifications = _notificationService.GetAll()
        };
    }
}
