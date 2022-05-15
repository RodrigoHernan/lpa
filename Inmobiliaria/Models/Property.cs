using System;
using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria.Models
{
    public class Property
    {
        public string? UserId { get; set; }

        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public int Price { get; set; }

        public int Taxes { get; set; }

        public int HouseSize { get; set; }

        public int Rooms { get; set; }

    }
}
