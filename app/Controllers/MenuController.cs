using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using app.Services;
using app.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace app.Controllers;

public class MenuController : Controller
{
    private readonly IPropertyService _propertyService;
    private readonly UserManager<ApplicationUser> _userManager;

    public MenuController(IPropertyService propertyService, UserManager<ApplicationUser> userManager)
    {
        _propertyService = propertyService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var items = await _propertyService.GetProperties();

        var model = new PropertyViewModel(){ Items = items };

        return View(model);
    }

    //get details
    public async Task<IActionResult> Details(string id)
    {
        //conver id to Guid
        var guid = Guid.Parse(id);
        var item = await _propertyService.GetProperty(guid);
        Property[] properties = new Property[1];
        properties[0] = item;


        var model = new PropertyViewModel() { Items = properties };

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
