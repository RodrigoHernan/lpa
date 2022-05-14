using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Inmobiliaria.Models;

namespace Inmobiliaria.Controllers;

public class PropertyController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

}
