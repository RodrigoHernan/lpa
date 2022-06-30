using Inmobiliaria.Data;
using Inmobiliaria.Models;
using Microsoft.EntityFrameworkCore;

namespace Inmobiliaria.Services
{
    public interface IClaimService
    {
        List<Claim> GetAll();
    }
    public class ClaimService : IClaimService
    {
        public List<Claim> GetAll()
        {
            return [];
            // return await _context.LogEntries.ToListAsync();
        }
    }
}
