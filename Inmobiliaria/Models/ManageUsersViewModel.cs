using System.Collections.Generic;

namespace Inmobiliaria.Models
{
    public class ManageUsersViewModel
    {
        public ApplicationUser[] Administrators { get; set; }

        public ApplicationUser[] Everyone { get; set;}

        // public ApplicationUser[] Roles { get; set;}
    }
}

