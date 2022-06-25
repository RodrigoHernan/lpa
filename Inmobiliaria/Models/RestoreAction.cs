using System;
using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria.Models
{
    public class RestoreAction
    {
        [Required]
        public int RestoreId { get; set; }
    }
}

