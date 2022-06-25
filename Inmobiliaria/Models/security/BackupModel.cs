
namespace Inmobiliaria.Models
{
    public class BackupModel {

        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime Fecha { get; set; }
        public string RutaDelArchivo { get; set; }

        public override string ToString()
        {
            return string.Format("{0} [{1:yyyy-MM-dd HH:mm:ss}] - {2}", this.Nombre, this.Fecha, this.RutaDelArchivo);
        }
    }
}