using Microsoft.AspNetCore.Mvc;
using NumberPlateWeb.Modules.Auth.Services;
using NumberPlateWeb.Modules.Auth.ViewModels;

namespace NumberPlateWeb.Controllers;

public class AuthController : Controller
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    public IActionResult Index()
    {
        return View(new LoginRequest());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", request);
        }

        var user = _authService.Login(request);

        if (user is null)
        {
            ModelState.AddModelError(string.Empty, "Tài khoản hoặc mật khẩu không hợp lệ.");
            return View("Index", request);
        }

        HttpContext.Session.SetString("DisplayName", user.DisplayName);
        HttpContext.Session.SetString("Role", user.Role);
        TempData["SuccessMessage"] = $"Đăng nhập thành công: {user.Role}.";

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction(nameof(Index));
    }
}
