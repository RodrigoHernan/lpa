using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Inmobiliaria.Services;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Inmobiliaria.Controllers;

public class PropertyController : Controller
{
    private readonly IPropertyService _propertyService;
    private readonly UserManager<ApplicationUser> _userManager;

    public PropertyController(IPropertyService propertyService, UserManager<ApplicationUser> userManager)
    {
        _propertyService = propertyService;
        _userManager = userManager;
    }

    public async Task<IActionResult>  Index()
    {
        var items = await _propertyService.GetProperties();

        var model = new PropertyViewModel(){ Items = items };

        return View(model);
    }

    [Authorize]
    public async Task<IActionResult> user()
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null) return Challenge();

        var items = await _propertyService.GetUserProperties(currentUser);

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
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null) return Challenge();


        var successful = await _propertyService.AddPropertyAsync(newProperty, currentUser);
        if (!successful)
        {
            return BadRequest("Could not add item.");
        }

        return RedirectToAction("User");
    }

}
