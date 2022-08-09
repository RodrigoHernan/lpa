using System;
using System.ComponentModel.DataAnnotations;

namespace app.Models
{
    public class Property:IEntidadConDigitoVerificador
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

        public DateTimeOffset? Created { get; set; }

        [Required]
        public string City { get; set; }

        public byte[]? DVH { get; set; }

    }
}
