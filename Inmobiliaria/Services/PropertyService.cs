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
        private readonly ILoggerService _logger;

        public PropertyService(ApplicationDbContext context, ILoggerService logger) {
            _context = context;
            _logger = logger;
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
            await _logger.Log(LogLevel.Debug, $"Adding property {newProperty.Title}");
            newProperty.Id = Guid.NewGuid();
            newProperty.Created = DateTimeOffset.Now;
            newProperty.UserId = user.Id;

            _context.Properties.Add(newProperty);

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }

        public async Task<Property> GetProperty(Guid id){

            var item = await _context.Properties.FirstOrDefaultAsync(x => x.Id == id);
            return item;
        }

        public async Task<bool> RemovePropertyAsync(Guid id)
        {
            var item = await _context.Properties
                .FirstOrDefaultAsync(x => x.Id == id);
            if (item == null)
            {
                return false;
            }
            _context.Properties.Remove(item);
            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }

        public async Task<bool> UpdatePropertyAsync(Property property)
        {
            var item = await _context.Properties
                .FirstOrDefaultAsync(x => x.Id == property.Id);
            if (item == null)
            {
                return false;
            }
            item.Title = property.Title;
            item.Description = property.Description;
            item.Price = property.Price;
            item.Rooms = property.Rooms;

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }
    }
}
