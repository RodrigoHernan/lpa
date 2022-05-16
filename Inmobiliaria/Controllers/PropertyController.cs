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

    public async Task<IActionResult>  Index()
    {
        var items = await _propertyService.GetProperties();

        var model = new PropertyViewModel(){ Items = items };

        return View(model);
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

    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddProperty(Property newProperty)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("User");
        }

        var successful = await _propertyService.AddPropertyAsync(newProperty);
        if (!successful)
        {
            return BadRequest("Could not add item.");
        }

        return RedirectToAction("User");
    }

}
