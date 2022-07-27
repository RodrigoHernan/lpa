using Inmobiliaria.Data;
using Inmobiliaria.Models;
using Microsoft.EntityFrameworkCore;

namespace Inmobiliaria.Services
{
    public interface IClaimService
    {
        Task<List<PermisoModel>> GetAll();
        Task<List<PermisoModel>> GetAll(ApplicationUser user);
        Task<List<PermisoModel>> GetEnabledPermissions(ApplicationUser user);
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

        public async Task<List<PermisoModel>> GetAll( ApplicationUser user)
        {
            var permisos = Enumerable.Empty<PermisoModel>().ToList();
            return permisos;
        }

        public async Task<List<PermisoModel>> GetEnabledPermissions( ApplicationUser user)
        {
            // var userPermisos = await _context.UserPermisos
            //     .Where(x => x.UserId == user.Id).ToListAsync();
            return Enumerable.Empty<PermisoModel>().ToList();
        }

        public async Task<bool> FillPermissionsUser( ApplicationUser user)
        {
            user.Permisos = await GetAll(user) ?? Enumerable.Empty<PermisoModel>().ToList();
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
                .Where(patente => !patente.Familia_Patentes.Any(fp => fp.FamiliaId == id))
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

    }
}
