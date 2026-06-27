using Microsoft.AspNetCore.Mvc;
using NumberPlateWeb.Modules.PoliceManagement.Services;
using NumberPlateWeb.Modules.PoliceManagement.ViewModels;

namespace NumberPlateWeb.Controllers;

public class PoliceController : Controller
{
    private readonly PoliceService _policeService;

    public PoliceController(PoliceService policeService)
    {
        _policeService = policeService;
    }

    public IActionResult Index()
    {
        return View(BuildViewModel(new PoliceOfficerInput()));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind(Prefix = "Input")] PoliceOfficerInput input)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", BuildViewModel(input));
        }

        _policeService.Create(input);
        TempData["SuccessMessage"] = "Đã thêm cảnh sát mới.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ToggleActive(int id)
    {
        _policeService.ToggleActive(id);
        return RedirectToAction(nameof(Index));
    }

    private PoliceIndexViewModel BuildViewModel(PoliceOfficerInput input)
    {
        return new PoliceIndexViewModel
        {
            Input = input,
            Officers = _policeService.GetAll()
        };
    }
}
