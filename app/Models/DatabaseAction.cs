using System;
using System.ComponentModel.DataAnnotations;

namespace app.Models
{
    public class DatabaseAction
    {
        [Required]
        public string Action { get; set; }
    }
}
