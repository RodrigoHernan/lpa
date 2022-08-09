using System;
using System.ComponentModel.DataAnnotations;

namespace app.Models
{
    public class RestoreAction
    {
        [Required]
        public int RestoreId { get; set; }
    }
}

