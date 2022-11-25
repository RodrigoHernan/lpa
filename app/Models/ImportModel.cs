using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace app.Models
{
    public class ApplicationUserView
    {
        [XmlIgnore()]
        [NotMapped]
        [DisplayName("Image")]
        public IFormFile? XmlFile { get; set; }
        public IEnumerable<app.Models.ApplicationUser> Objects { get; set; }
    }
}
