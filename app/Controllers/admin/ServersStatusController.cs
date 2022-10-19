using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using app.Models;

namespace app.Controllers;

public class ServersStatusController : Controller
{
    private readonly ILogger<ServersStatusController> _logger;

    public ServersStatusController(ILogger<ServersStatusController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
}
