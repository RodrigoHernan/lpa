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
    public class UserPermissionApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserPermissionApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/UserPermissionApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserPermission>>> GetUserPermissions()
        {
          if (_context.UserPermissions == null)
          {
              return NotFound();
          }
            return await _context.UserPermissions.ToListAsync();
        }

        // GET: api/UserPermissionApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserPermission>> GetUserPermission(int id)
        {
          if (_context.UserPermissions == null)
          {
              return NotFound();
          }
            var userPermission = await _context.UserPermissions.FindAsync(id);

            if (userPermission == null)
            {
                return NotFound();
            }

            return userPermission;
        }

        // PUT: api/UserPermissionApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserPermission(int id, UserPermission userPermission)
        {
            if (id != userPermission.Id)
            {
                return BadRequest();
            }

            _context.Entry(userPermission).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserPermissionExists(id))
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

        // POST: api/UserPermissionApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserPermission>> PostUserPermission(UserPermission userPermission)
        {
          if (_context.UserPermissions == null)
          {
              return Problem("Entity set 'ApplicationDbContext.UserPermissions'  is null.");
          }
            _context.UserPermissions.Add(userPermission);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserPermission", new { id = userPermission.Id }, userPermission);
        }

        // DELETE: api/UserPermissionApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserPermission(int id)
        {
            if (_context.UserPermissions == null)
            {
                return NotFound();
            }
            var userPermission = await _context.UserPermissions.FindAsync(id);
            if (userPermission == null)
            {
                return NotFound();
            }

            _context.UserPermissions.Remove(userPermission);
            await _context.SaveChangesAsync();

            HttpContext.Response.Headers.Add("HX-Trigger", "item-updated");
            return NoContent();
        }

        private bool UserPermissionExists(int id)
        {
            return (_context.UserPermissions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
