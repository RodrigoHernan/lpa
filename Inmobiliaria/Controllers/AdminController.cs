using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Inmobiliaria.Models;

namespace Inmobiliaria.Controllers;

public class AdminController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
