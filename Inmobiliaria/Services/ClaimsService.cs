using Inmobiliaria.Data;
using Inmobiliaria.Models;
using Microsoft.EntityFrameworkCore;

namespace Inmobiliaria.Services
{
    public interface IClaimService
    {
        Task<List<ClaimModel>> GetAll();
    }
    public class ClaimService : IClaimService
    {
        public Task<List<ClaimModel>> GetAll()
        {
            // return [];
            // return await _context.LogEntries.ToListAsync();
            return null;
        }
    }
}
