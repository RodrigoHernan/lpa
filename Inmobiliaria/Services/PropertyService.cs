using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Inmobiliaria.Data;
using Inmobiliaria.Models;

namespace Inmobiliaria.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly ApplicationDbContext _context;

        public PropertyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Property[]> GetUserProperties()
        {
            var items = await _context.Properties
                // .Where(x => x.IsDone == false)
                .ToArrayAsync();
            return items;
        }
    }
}
