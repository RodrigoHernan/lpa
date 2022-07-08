using Inmobiliaria.Data;
using Inmobiliaria.Models;
using Microsoft.EntityFrameworkCore;

namespace Inmobiliaria.Services
{
    public interface IClaimService
    {
        Task<List<PermisoModel>> GetAll();
        Task<List<FamiliaModel>> GetAllFamilies();
        Task<bool> CreateFamily(FamiliaModel family);
        Task<FamiliaModel> GetFamilyById(int? id);
        Task<bool> UpdateFamily(FamiliaModel family);
        bool FamiliaModelExists(int id);
        Task<bool> DeleteFamily(FamiliaModel family);
    }
    public class ClaimService : IClaimService
    {
        private readonly ApplicationDbContext _context;

        public ClaimService(ApplicationDbContext context) {
            _context = context;
        }

        public Task<List<PermisoModel>> GetAll()
        {
            return null;
        }

        public async Task<List<FamiliaModel>> GetAllFamilies()
        {
            return await _context.Familias.ToListAsync();
        }

        public async Task<bool> CreateFamily(FamiliaModel family)
        {
            _context.Familias.Add(family);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<FamiliaModel> GetFamilyById(int? id)
        {
            if (id == null)
            {
                return null;
            }
            return await _context.Familias.FindAsync(id);
        }

        public async Task<bool> UpdateFamily(FamiliaModel family)
        {
            _context.Familias.Update(family);
            await _context.SaveChangesAsync();
            return true;
        }

        public bool FamiliaModelExists(int id)
        {
          return (_context.Familias?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<bool> DeleteFamily(FamiliaModel family)
        {
            _context.Familias.Remove(family);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
