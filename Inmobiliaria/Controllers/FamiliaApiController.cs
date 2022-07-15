using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inmobiliaria.Data;
using Inmobiliaria.Models;

namespace Inmobiliaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamiliaApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FamiliaApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/FamiliaApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FamiliaModel>>> GetFamilias()
        {
          if (_context.Familias == null)
          {
              return NotFound();
          }
            return await _context.Familias.ToListAsync();
        }

        // GET: api/FamiliaApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FamiliaModel>> GetFamiliaModel(int id)
        {
          if (_context.Familias == null)
          {
              return NotFound();
          }
            var familiaModel = await _context.Familias.FindAsync(id);

            if (familiaModel == null)
            {
                return NotFound();
            }

            return familiaModel;
        }

        // PUT: api/FamiliaApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFamiliaModel(int id, FamiliaModel familiaModel)
        {
            if (id != familiaModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(familiaModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FamiliaModelExists(id))
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

        // POST: api/FamiliaApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FamiliaModel>> PostFamiliaModel(FamiliaModel familiaModel)
        {
          if (_context.Familias == null)
          {
              return Problem("Entity set 'ApplicationDbContext.Familias'  is null.");
          }
            _context.Familias.Add(familiaModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFamiliaModel", new { id = familiaModel.Id }, familiaModel);
        }

        // DELETE: api/FamiliaApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFamiliaModel(int id)
        {
            if (_context.Familias == null)
            {
                return NotFound();
            }
            var familiaModel = await _context.Familias.FindAsync(id);
            if (familiaModel == null)
            {
                return NotFound();
            }

            _context.Familias.Remove(familiaModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FamiliaModelExists(int id)
        {
            return (_context.Familias?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
