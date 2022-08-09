using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inmobiliaria.Data;
using Inmobiliaria.Models;
using Inmobiliaria.Policy;
using Inmobiliaria.Services;
using Microsoft.AspNetCore.Authorization;


namespace Inmobiliaria.Controllers;

// [Authorize(Policy = "PatenteFamilia", Patente = "PuedeAdministrarRolesyPermisos")]
public class AdminController : Controller
{
    private readonly ILoggerService _logger;
    private readonly IClaimService _claims;

    public AdminController(ILoggerService logger, IClaimService claims) {
        _logger = logger;
        _claims = claims;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Logs(string? user, DateTime? startDate, DateTime? endDate) {
        List<LogEntry> items;
        if (await _claims.hasAccess(HttpContext.User, TipoPermiso.PuedeAdministrarLogs)) {
            return RedirectToPage("/Identity/Account/AccessDenied");
        }

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
        if (!await _claims.hasAccess(HttpContext.User, TipoPermiso.PuedeAdministrarRolesyPermisos)) return Redirect("/Identity/Account/AccessDenied");

        var model = new ClaimsViewModel(){
            Families = await _claims.GetAllFamilies(),
            Patentes = await _claims.GetAllPatentes()
        };
        return View(model);
    }

     // GET: Admin/Details/5
    public async Task<IActionResult> FamilyDetails(int id)
    {
        if (!await _claims.hasAccess(HttpContext.User, TipoPermiso.PuedeAdministrarRolesyPermisos)) return Redirect("/Identity/Account/AccessDenied");
        if (id == null)
        {
            return NotFound();
        }

        var familiaModel = await _claims.GetFamilyById(id);
        if (familiaModel == null)
        {
            return NotFound();
        }

        return View("Permisos/FamilyDetails", familiaModel);
    }

    // GET: Admin/CreateFamily
    public async Task<IActionResult> CreateFamily()
    {
        if (!await _claims.hasAccess(HttpContext.User, TipoPermiso.PuedeAdministrarRolesyPermisos)) return Redirect("/Identity/Account/AccessDenied");
        return View("Permisos/CreateFamily");
    }

    // POST: Admin/CreateFamily
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateFamily([Bind("Id,Nombre,TipoPermiso")] FamiliaModel familiaModel)
    {
        if (!await _claims.hasAccess(HttpContext.User, TipoPermiso.PuedeAdministrarRolesyPermisos)) return Redirect("/Identity/Account/AccessDenied");

        if (ModelState.IsValid)
        {
            await _claims.CreateFamily(familiaModel);
            return RedirectToAction(nameof(Claims));
        }
        return View("Permisos/CreateFamily", familiaModel);
    }

    // GET: Admin/Edit/5
    public async Task<IActionResult> EditFamily(int? id)
    {
        if (!await _claims.hasAccess(HttpContext.User, TipoPermiso.PuedeAdministrarRolesyPermisos)) return Redirect("/Identity/Account/AccessDenied");

        if (id == null)
        {
            return NotFound();
        }
        var familiaModel = await _claims.GetFamilyById(id.Value);
        if (familiaModel == null)
        {
            return NotFound();
        }
        return View("Permisos/EditFamily", familiaModel);
    }

    // POST: Admin/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditFamily(int id, [Bind("Id,Nombre,TipoPermiso")] FamiliaModel familiaModel)
    {
        if (!await _claims.hasAccess(HttpContext.User, TipoPermiso.PuedeAdministrarRolesyPermisos)) return Redirect("/Identity/Account/AccessDenied");

        if (id != familiaModel.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                await _claims.UpdateFamily(familiaModel);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_claims.FamiliaModelExists(familiaModel.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Claims));
        }
        familiaModel = await _claims.GetFamilyById(id);
        return View("Permisos/EditFamily", familiaModel);
    }

    // GET: Admin/Delete/5
    public async Task<IActionResult> DeleteFamily(int id)
    {
        if (!await _claims.hasAccess(HttpContext.User, TipoPermiso.PuedeAdministrarRolesyPermisos)) return Redirect("/Identity/Account/AccessDenied");

        if (id == null)
        {
            return NotFound();
        }

        var familiaModel = await _claims.GetFamilyById(id);
        if (familiaModel == null)
        {
            return NotFound();
        }

        return View("Permisos/DeleteFamily", familiaModel);
    }

    // POST: Admin/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteFamilyConfirmed(int id)
    {
        if (!await _claims.hasAccess(HttpContext.User, TipoPermiso.PuedeAdministrarRolesyPermisos)) return Redirect("/Identity/Account/AccessDenied");

        var familiaModel = await _claims.GetFamilyById(id);
        if (familiaModel != null)
        {
            await _claims.DeleteFamily(familiaModel);
        }

        return RedirectToAction(nameof(Claims));
    }

}
