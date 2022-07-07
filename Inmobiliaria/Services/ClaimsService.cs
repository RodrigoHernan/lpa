using Inmobiliaria.Data;
using Inmobiliaria.Models;
using Microsoft.EntityFrameworkCore;

namespace Inmobiliaria.Services
{
    public interface IClaimService
    {
        Task<List<PermisoModel>> GetAll();
    }
    public class ClaimService : IClaimService
    {
        public Task<List<PermisoModel>> GetAll()
        {
            // return [];
            return null;
        }
    }
}
