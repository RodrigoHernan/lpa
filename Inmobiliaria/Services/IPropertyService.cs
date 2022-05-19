using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Identity;

namespace Inmobiliaria.Services
{
    public interface IPropertyService
    {
        Task<Property[]> GetUserProperties(ApplicationUser user);
        Task<Property[]> GetProperties();
        Task<bool> AddPropertyAsync(Property newProperty, ApplicationUser user);
    }
}
