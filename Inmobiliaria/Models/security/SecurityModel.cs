
namespace Inmobiliaria.Models
{

    public abstract class PermisoModel {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
    }

    public class FamiliaModel : PermisoModel {
        // public List<PermisoModel> Permisos { get; set; }
        public List<Familia_Patente>? Familia_Patentes { get; set; }
    }

    public class Patente : PermisoModel {
        public List<Familia_Patente> Familia_Patentes { get; set; }

    }


    public class Familia_Patente {
        public int Id { get; set; }
        public int FamiliaId { get; set; }
        public FamiliaModel Familia { get; set; }

        public int PatenteId { get; set; }
        public Patente Patente { get; set; }
    }
}