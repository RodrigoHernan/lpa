using System;
using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria.Models
{
    public class DatabaseAction
    {
        [Required]
        public string Action { get; set; }
    }
}
