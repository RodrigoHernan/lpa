using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Inmobiliaria.Models;
using Inmobiliaria.Services;

namespace Inmobiliaria.Controllers;

public class AdminController : Controller
{
    private readonly ILoggerService _logger;
    private readonly IClaimService _claims;

    public AdminController(ILoggerService logger, IClaimService _claims) {
        _logger = logger;
        _claims = _claims;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Logs(string? user) {

        var items = await _logger.GetAllLogs();
        var model = new LogEntryViewModel(){ Items = items };
        return View(model);
    }


    public async Task<IActionResult> Claims(string? user) {

        var items = await _claims.GetAll();
        // var model = new ClaimsViewModel(){ Items = items };
        return View();
    }

}
