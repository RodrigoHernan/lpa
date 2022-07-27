using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Inmobiliaria.Models
{
    public class ApplicationUser : IdentityUser
    {
        // public virtual string Email { get; set; } // example, not necessary
        // public Role Role { get; set; }
        public int InvalidPasswordAttempt { get; set; }

        public List<PermisoModel>? Permisos { get; set; }
    }

    public static class Role
    {
        public const string Admin = "Administrator";
        public const string User = "user";
    }

}
