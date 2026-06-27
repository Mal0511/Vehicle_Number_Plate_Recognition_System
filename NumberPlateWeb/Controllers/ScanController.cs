using Microsoft.AspNetCore.Mvc;
using NumberPlateWeb.Modules.Scanning.Services;
using NumberPlateWeb.Modules.Scanning.ViewModels;

namespace NumberPlateWeb.Controllers;

public class ScanController : Controller
{
    private readonly ScanService _scanService;

    public ScanController(ScanService scanService)
    {
        _scanService = scanService;
    }

    public IActionResult Index()
    {
        return View(BuildViewModel(new ScanRequest()));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Run([Bind(Prefix = "Input")] ScanRequest input)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", BuildViewModel(input));
        }

        var result = await _scanService.ScanAsync(input);

        return View("Index", new ScanIndexViewModel
        {
            Input = input,
            Result = result,
            Logs = _scanService.GetRecentLogs()
        });
    }

    private ScanIndexViewModel BuildViewModel(ScanRequest input)
    {
        return new ScanIndexViewModel
        {
            Input = input,
            Logs = _scanService.GetRecentLogs()
        };
    }
}
