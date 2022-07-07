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

    public async Task<IActionResult> Logs(string? user, DateTime? startDate, DateTime? endDate) {
        List<LogEntry> items;
        if (startDate != null || endDate != null) {
            items = await _logger.GetLogsByDateRange(
                startDate ?? new DateTime(1000, 1, 1),
                endDate ?? new DateTime(2099, 1, 1)
            );
        } else {
            items = await _logger.GetAllLogs();
        }

        var model = new LogEntryViewModel(){
            Items = items,
            StartDate = startDate,
            EndDate = endDate
        };
        return View(model);
    }


    public async Task<IActionResult> Claims() {

        // var items = await _claims.GetAll();
        // var model = new ClaimsViewModel(){ Items = items };
        return View();
    }

}
