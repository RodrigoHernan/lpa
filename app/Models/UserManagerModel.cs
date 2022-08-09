using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace app.Models
{
    public class ApplicationUser : IdentityUser
    {
        // public virtual string Email { get; set; } // example, not necessary
        // public Role Role { get; set; }
        public int InvalidPasswordAttempt { get; set; }

        public List<UserPermission> Permisos { get; set; } = new List<UserPermission>();
    }

    public static class Role
    {
        public const string Admin = "Administrator";
        public const string User = "user";
    }

}
