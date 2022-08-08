using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inmobiliaria.Data;
using Inmobiliaria.Models;
using Inmobiliaria.Services;

namespace Inmobiliaria.Controllers
{
    public class ApplicationUserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IClaimService _claims;


        public ApplicationUserController(ApplicationDbContext context, IClaimService claims) {
            _claims = claims;
            _context = context;
        }

        // GET: ApplicationUser
        public async Task<IActionResult> Index()
        {
              return _context.users != null ?
                          View(await _context.users.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.users'  is null.");
        }

        // GET: ApplicationUser/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.users == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        // GET: ApplicationUser/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ApplicationUser/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InvalidPasswordAttempt,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(applicationUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(applicationUser);
        }

        // GET: ApplicationUser/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.users == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.users.FindAsync(id);
            if (applicationUser == null)
            {
                return NotFound();
            }
            return View(applicationUser);
        }

        // POST: ApplicationUser/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("InvalidPasswordAttempt,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] ApplicationUser applicationUser)
        {
            if (id != applicationUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicationUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(applicationUser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(applicationUser);
        }

        // GET: ApplicationUser/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.users == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        // POST: ApplicationUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.users == null)
            {
                return Problem("Entity set 'ApplicationDbContext.users'  is null.");
            }
            var applicationUser = await _context.users.FindAsync(id);
            if (applicationUser != null)
            {
                _context.users.Remove(applicationUser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationUserExists(string id)
        {
          return (_context.users?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> EditPermissions(string id)
        {
            if (id == null || _context.users == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.users.FindAsync(id);
            if (applicationUser == null)
            {
                return NotFound();
            }
            await _claims.FillPermissionsUser(applicationUser);


            EditPermissionViewModel model = new EditPermissionViewModel(){
                Id = applicationUser.Id,
                Permisos = applicationUser.Permisos,
                PermisosDisponibles = await _claims.GetEnabledPermissions(applicationUser),
            };
            return View(model);
        }

        // POST: ApplicationUser/EditPermissionsPost/5
        [HttpPost, ActionName("EditPermissionsPost")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPermissionsPost(string id, int PermisoId)
        {
            if (_context.users == null)
            {
                return Problem("Entity set 'ApplicationDbContext.users'  is null.");
            }

            if (PermisoId == 0)
            {
                return RedirectToAction(nameof(EditPermissions), new { id = id });
            }

            var applicationUser = await _context.users.FindAsync(id);

            UserPermission userPermission = new UserPermission(){
                ApplicationUserId = applicationUser.Id,
                PermisoId = PermisoId,
            };

            applicationUser.Permisos.Add(userPermission);
            await _context.SaveChangesAsync();
            HttpContext.Response.Headers.Add("HX-Trigger", "item-updated");
            return NoContent();
        }

    }
}
