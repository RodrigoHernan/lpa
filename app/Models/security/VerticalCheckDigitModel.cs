using System.ComponentModel.DataAnnotations;

namespace app.Models
{
    public class VerticalCheckDigit
    {
        [Key]
        public string Entity { get; set; }
        public byte[] Checksum { get; set; }

        public string IdsTracker { get; set; }

    }
}