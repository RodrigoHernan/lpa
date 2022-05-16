using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Inmobiliaria.Models;

namespace Inmobiliaria.Services
{
    public interface IPropertyService
    {
        Task<Property[]> GetUserProperties();
        Task<Property[]> GetProperties();
        Task<bool> AddPropertyAsync(Property newProperty);
    }
}
