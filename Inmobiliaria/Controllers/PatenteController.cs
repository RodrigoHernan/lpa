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
    public class PatenteController : Controller
    {
        private readonly IClaimService _claims;

        public PatenteController(IClaimService claims) {
            _claims = claims;
        }


        // GET: Patente
        public async Task<IActionResult> Index()
        {
            List<Patente> Patentes = await _claims.GetAllPatentes();
            return View(Patentes);
        }

        // GET: Patente/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patente = await _claims.GetPatenteById(id);
            if (patente == null)
            {
                return NotFound();
            }

            return View(patente);
        }

        // GET: Patente/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Patente/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,TipoPermiso")] Patente patente)
        {
            if (ModelState.IsValid)
            {
                await _claims.CreatePatente(patente);
                return Redirect(Url.Action("Claims", "Admin") + "#Patente");
            }
            return View(patente);
        }

        // GET: Patente/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patente = await _claims.GetPatenteById(id);
            if (patente == null)
            {
                return NotFound();
            }
            return View(patente);
        }

        // POST: Patente/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,TipoPermiso")] Patente patente)
        {
            if (id != patente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _claims.UpdatePatente(patente);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_claims.PatenteExists(patente.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect(Url.Action("Claims", "Admin") + "#Patente");
            }
            return View(patente);
        }

        // GET: Patente/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patente = await _claims.GetPatenteById(id);
            if (patente == null)
            {
                return NotFound();
            }

            return View(patente);
        }

        // POST: Patente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patente = await _claims.GetPatenteById(id);
            if (patente != null)
            {
                await _claims.DeletePatente(patente);
            }
            return Redirect(Url.Action("Claims", "Admin") + "#Patente");
        }
    }
}
