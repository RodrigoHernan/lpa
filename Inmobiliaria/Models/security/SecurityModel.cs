
namespace Inmobiliaria.Models
{

    public abstract class PermisoModel {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
    }

    public class FamiliaModel : PermisoModel {
        // public List<PermisoModel> Permisos { get; set; }
    }

    public class PatenteModel : PermisoModel {
    }
}