using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria.Models
{
    public class VerticalCheckDigit
    {
        [Key]
        public string Entity { get; set; }
        public byte[] Checksum { get; set; }
    }
}