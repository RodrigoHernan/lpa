using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Inmobiliaria.Services;
using Inmobiliaria.Models;

namespace Inmobiliaria.Controllers;

public class PropertyController : Controller
{
    private readonly IPropertyService _propertyService;

    public PropertyController(IPropertyService propertyService)
    {
        _propertyService = propertyService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> User()
    {
        var items = await _propertyService.GetUserProperties();

        var model = new PropertyViewModel()
        {
            Items = items
        };

        return View(model);
    }

}
