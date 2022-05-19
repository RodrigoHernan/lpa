using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Inmobiliaria.Data;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Identity;

namespace Inmobiliaria.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly ApplicationDbContext _context;

        public PropertyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Property[]> GetUserProperties(ApplicationUser user)
        {
            var items = await _context.Properties
                .Where(x => x.UserId == user.Id)
                .ToArrayAsync();
            return items;
        }

        public async Task<Property[]> GetProperties()
        {
            var items = await _context.Properties
                .ToArrayAsync();
            return items;
        }

        public async Task<bool> AddPropertyAsync(Property newProperty, ApplicationUser user)
        {
            newProperty.Id = Guid.NewGuid();
            newProperty.Created = DateTimeOffset.Now.AddDays(3);
            newProperty.UserId = user.Id;

            _context.Properties.Add(newProperty);

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }

    }
}
