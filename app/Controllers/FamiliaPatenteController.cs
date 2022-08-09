using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using app.Data;
using app.Models;
using app.Services;

namespace app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamiliaPatenteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IClaimService _claims;

        public FamiliaPatenteController(ApplicationDbContext context, IClaimService claims)
        {
            _context = context;
            _claims = claims;
        }

        // GET: api/FamiliaPatente
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Familia_Patente>>> GetFamiliasPatente()
        {
          if (_context.FamiliasPatente == null)
          {
              return NotFound();
          }
            return await _context.FamiliasPatente.ToListAsync();
        }

        // GET: api/FamiliaPatente/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Familia_Patente>> GetFamilia_Patente(int id)
        {
          if (_context.FamiliasPatente == null)
          {
              return NotFound();
          }
            var familia_Patente = await _context.FamiliasPatente.FindAsync(id);

            if (familia_Patente == null)
            {
                return NotFound();
            }

            return familia_Patente;
        }

        // PUT: api/FamiliaPatente/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFamilia_Patente(int id, Familia_Patente familia_Patente)
        {
            if (id != familia_Patente.Id)
            {
                return BadRequest();
            }

            _context.Entry(familia_Patente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Familia_PatenteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/FamiliaPatente
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Familia_Patente>> PostFamilia_Patente(Familia_Patente familia_Patente)
        {
          if (_context.FamiliasPatente == null)
          {
              return Problem("Entity set 'ApplicationDbContext.FamiliasPatente'  is null.");
          }
            _context.FamiliasPatente.Add(familia_Patente);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFamilia_Patente", new { id = familia_Patente.Id }, familia_Patente);
        }

        // DELETE: api/FamiliaPatente/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FamilyFamiliaPatenteModel>> DeleteFamilia_Patente(int id)
        {
            if (_context.FamiliasPatente == null)
            {
                return NotFound();
            }
            var familia_Patente = await _context.FamiliasPatente.FindAsync(id);
            if (familia_Patente == null)
            {
                return NotFound();
            }

            _context.FamiliasPatente.Remove(familia_Patente);
            await _context.SaveChangesAsync();

            return await _claims.GetFamiliaPatenteViewModel(familia_Patente.FamiliaId);
        }

        private bool Familia_PatenteExists(int id)
        {
            return (_context.FamiliasPatente?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
