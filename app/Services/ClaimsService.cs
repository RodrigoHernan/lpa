using app.Data;
using app.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace app.Services
{
    public interface IClaimService
    {
        Task<List<Permiso>> GetAll();
        Task<List<Permiso>> GetAll(ApplicationUser user);
        Task<List<Permiso>> GetEnabledPermissions(ApplicationUser user);
        Task<bool> FillPermissionsUser( ApplicationUser user);
        Task<List<FamiliaModel>> GetAllFamilies();
        Task<bool> CreateFamily(FamiliaModel family);
        Task<FamiliaModel> GetFamilyById(int? id);
        Task<bool> UpdateFamily(FamiliaModel family);
        bool FamiliaModelExists(int id);
        Task<bool> DeleteFamily(FamiliaModel family);
        Task<bool> CreatePatente(Patente patente);

        Task<List<Patente>> GetAllPatentes();
        Task<Patente> GetPatenteById(int? id);
        Task<bool> UpdatePatente(Patente patente);
        bool PatenteExists(int id);
        Task<bool> DeletePatente(Patente patente);

        Task<List<Familia_Patente>> GetFamiliasPatentesByFamilyiD(int id);
        Task<List<Patente>> GetPatentesDisponiblesByFamilyiD(int id);
        Task<FamilyFamiliaPatenteModel> GetFamiliaPatenteViewModel(int id);
        Task<bool> AddPatenteToFamily(int id, int patenteId);
        Task<bool> hasAccess(ClaimsPrincipal user, TipoPermiso permiso);
    }
    public class ClaimService : IClaimService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ClaimService(ApplicationDbContext context, UserManager<ApplicationUser> userManager) {
            _context = context;
            _userManager = userManager;
        }

        public Task<List<Permiso>> GetAll()
        {
            return null;
        }

        public async Task<List<Permiso>> GetAll( ApplicationUser user)
        {
            return await _context.Permisos
                .Where(permiso => permiso.Users.Any(fp => fp.ApplicationUserId == user.Id))
                .ToListAsync();
        }

        public async Task<List<Permiso>> GetEnabledPermissions( ApplicationUser user)
        {
            return await _context.Permisos
                .Where(permiso => !permiso.Users.Any(fp => fp.ApplicationUserId == user.Id))
                .ToListAsync();
        }

        public async Task<bool> FillPermissionsUser( ApplicationUser user)
        {
            user.Permisos = await _context.UserPermissions
                .Where(fp => fp.ApplicationUserId == user.Id)
                .Include(fp => fp.Permiso)
                .ToListAsync();
            return true;
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
            return await _context.Familias
                .Include(familia => familia.Familia_Patentes)
                .ThenInclude(fp => fp.Patente).FirstOrDefaultAsync(p => p.Id == id);
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

        #region Patentes
        public async Task<bool> CreatePatente(Patente patente)
        {
            _context.Patentes.Add(patente);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Patente>> GetAllPatentes()
        {
            return await _context.Patentes.ToListAsync();
        }

        public async Task<Patente> GetPatenteById(int? id)
        {
            if (id == null)
            {
                return null;
            }
            return await _context.Patentes.FindAsync(id);
        }

        public async Task<bool> UpdatePatente(Patente patente)
        {
            _context.Patentes.Update(patente);
            await _context.SaveChangesAsync();
            return true;
        }

        public bool PatenteExists(int id)
        {
            return (_context.Patentes?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<bool> DeletePatente(Patente patente)
        {
            _context.Patentes.Remove(patente);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Familia_Patente>> GetFamiliasPatentesByFamilyiD(int id)
        {
            return await _context.FamiliasPatente.Where(fp => fp.FamiliaId == id).ToListAsync();
        }

        public async Task<List<Patente>> GetPatentesDisponiblesByFamilyiD(int id)
        {
            return await _context.Patentes
                // .Where(patente => !patente.Familia_Patentes.Any(fp => fp.FamiliaId == id))
                .ToListAsync();
        }

        public async Task<bool> AddPatenteToFamily(int id, int patenteId)
        {
            var familia = await _context.Familias.FindAsync(id);
            var patente = await _context.Patentes.FindAsync(patenteId);
            if (familia == null || patente == null)
            {
                return false;
            }
            _context.FamiliasPatente.Add(new Familia_Patente {
                FamiliaId = id,
                PatenteId = patenteId
            });
            await _context.SaveChangesAsync();
            return true;
        }

        #endregion

        public async Task<FamilyFamiliaPatenteModel> GetFamiliaPatenteViewModel(int id)
        {
            var family = await GetFamilyById(id);
            var familiasPatentes = await GetFamiliasPatentesByFamilyiD(id);
            var patentesDisponibles = await GetPatentesDisponiblesByFamilyiD(id);

            var familiaPatente = new FamilyFamiliaPatenteModel(){
                Familia = family,
                FamiliasPatentes = familiasPatentes,
                PatentesDisponibles = patentesDisponibles
            };

            return familiaPatente;
        }

        public async Task<bool> hasAccess(ClaimsPrincipal ClaimUser, TipoPermiso permiso)
        {
            var user = await _userManager.GetUserAsync(ClaimUser);
            if (user == null)
            {
                return false;
            }
            var permisos = await GetAll(user);
            return permisos.Any(p => p.TipoPermiso == permiso || p.TipoPermiso == TipoPermiso.SuperUser);
        }
    }
}
