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

    public IWebHostEnvironment _IWebHostEnvironment { get; }

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
            var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y=>y.Count>0)
                           .ToList();
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

    public async Task<IActionResult> EditDish(Guid id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var dish = await _dishService.GetDish(id);
        if (dish == null)
        {
            return NotFound();
        }
        return View(dish);
    }

    // POST: Dish/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to, for
    // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditDish(Guid id, [Bind("Id,Title,Name,Description,Price,ImageFile")] Dish dish)
    {
        if (id != dish.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            await _dishService.UpdateDishAsync(dish);
            return RedirectToAction("User");
        }
        return View(dish);
    }

    // POST: Patente/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteDish(Guid id)
    {
        await _dishService.RemoveDishAsync(id);
        return RedirectToAction("User");
    }
}
