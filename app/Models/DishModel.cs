using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace app.Models
{
    public class Dish:IEntidadConDigitoVerificador
    {
        public string? UserId { get; set; }

        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public int Price { get; set; }

        public DateTimeOffset? Created { get; set; }

        public string? ImageName { get; set; }

        [XmlIgnore()]
        [NotMapped]
        [DisplayName("Image")]
        public IFormFile? ImageFile { get; set; }


        public byte[]? DVH { get; set; }

    }
}
