using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using app.Services;
using app.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace app.Controllers;

public class MenuController : Controller
{
    private readonly IDishService _dishService;
    private readonly UserManager<ApplicationUser> _userManager;

    public MenuController(IDishService dishService,
                          UserManager<ApplicationUser> userManager)
    {
        _dishService = dishService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var items = await _dishService.GetDishes();

        var model = new DishViewModel(){ Items = items };

        return View(model);
    }

    //get details
    public async Task<IActionResult> Details(string id)
    {
        //conver id to Guid
        var guid = Guid.Parse(id);
        var item = await _dishService.GetDish(guid);
        Dish[] properties = new Dish[1];
        properties[0] = item;


        var model = new DishViewModel() { Items = properties };

        return View(model);
    }

    [Authorize]
    public async Task<IActionResult> user()
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null) return Challenge();

        var items = await _dishService.GetDishes(currentUser);

        var model = new DishViewModel()
        {
            Items = items
        };

        return View(model);
    }

    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddDish(Dish newDish)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("User");
        }
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null) return Challenge();

        var successful = await _dishService.AddDishAsync(newDish, currentUser);
        if (!successful)
        {
            return BadRequest("Could not add item.");
        }

        return RedirectToAction("User");
    }

}
