using System.Collections.Generic;

namespace app.Models
{
    public class ManageUsersViewModel
    {
        public ApplicationUser[] Administrators { get; set; }

        public ApplicationUser[] Everyone { get; set;}

        // public ApplicationUser[] Roles { get; set;}
    }
}

