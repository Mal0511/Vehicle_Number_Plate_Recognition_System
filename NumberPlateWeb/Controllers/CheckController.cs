using Microsoft.AspNetCore.Mvc;
using NumberPlateWeb.Modules.CheckList.ViewModels;
using NumberPlateWeb.Modules.Shared;
using NumberPlateWeb.Modules.VehicleLists.Services;

namespace NumberPlateWeb.Controllers;

public class CheckController : Controller
{
    private readonly VehicleListService _vehicleListService;

    public CheckController(VehicleListService vehicleListService)
    {
        _vehicleListService = vehicleListService;
    }

    public IActionResult Index()
    {
        return View(BuildViewModel(new PlateCheckRequest(), null));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Run([Bind(Prefix = "Input")] PlateCheckRequest input)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", BuildViewModel(input, null));
        }

        var vehicle = _vehicleListService.FindByPlate(input.PlateNumber);
        var normalizedPlate = PlateNumberFormatter.Normalize(input.PlateNumber);

        var result = vehicle is null
            ? new PlateCheckResult
            {
                PlateNumber = normalizedPlate,
                Status = "Not Found",
                Found = false
            }
            : new PlateCheckResult
            {
                PlateNumber = vehicle.RegisteredNumber,
                Status = vehicle.Type.ToDisplayName(),
                Color = vehicle.Color,
                OwnerName = vehicle.OwnerName,
                Reason = vehicle.Reason,
                Found = true
            };

        return View("Index", BuildViewModel(input, result));
    }

    private CheckIndexViewModel BuildViewModel(PlateCheckRequest input, PlateCheckResult? result)
    {
        return new CheckIndexViewModel
        {
            Input = input,
            Result = result,
            SeedData = _vehicleListService.GetAll()
        };
    }
}
