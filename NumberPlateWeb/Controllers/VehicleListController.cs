using Microsoft.AspNetCore.Mvc;
using NumberPlateWeb.Modules.VehicleLists.Services;
using NumberPlateWeb.Modules.VehicleLists.ViewModels;

namespace NumberPlateWeb.Controllers;

public class VehicleListController : Controller
{
    private readonly VehicleListService _vehicleListService;

    public VehicleListController(VehicleListService vehicleListService)
    {
        _vehicleListService = vehicleListService;
    }

    public IActionResult Index()
    {
        return View(BuildViewModel(new VehicleRecordInput()));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Save([Bind(Prefix = "Input")] VehicleRecordInput input)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", BuildViewModel(input));
        }

        _vehicleListService.Save(input);
        TempData["SuccessMessage"] = "Đã lưu xe vào danh sách.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        _vehicleListService.Delete(id);
        return RedirectToAction(nameof(Index));
    }

    private VehicleListIndexViewModel BuildViewModel(VehicleRecordInput input)
    {
        return new VehicleListIndexViewModel
        {
            Input = input,
            Vehicles = _vehicleListService.GetAll()
        };
    }
}
