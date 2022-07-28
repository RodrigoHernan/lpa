
namespace Inmobiliaria.Models
{

    public abstract class Permiso {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }

        public IList<UserPermission> Users { get; set; }

        public abstract IList<Permiso> Hijos { get; }
        public abstract void AgregarHijo(Permiso c);
        public abstract void VaciarHijos();

        public override string ToString()
        {
            return Nombre;
        }
    }

    public class FamiliaModel : Permiso {
        private IList<Permiso> _hijos;

        public FamiliaModel()
        {
            _hijos = new List<Permiso>();
        }

        public override IList<Permiso> Hijos
        {
            get
            {
                return _hijos.ToArray();
            }
        }

        public override void VaciarHijos()
        {
            _hijos = new List<Permiso>();
        }
        public override void AgregarHijo(Permiso c)
        {
            _hijos.Add(c);
        }
        public List<Familia_Patente>? Familia_Patentes { get; set; }
    }

    public class Patente : Permiso {
        // public List<Familia_Patente> Familia_Patentes { get; set; }
        public override IList<Permiso> Hijos {
            get {
                return new List<Permiso>();
            }
        }
        public override void AgregarHijo(Permiso c) {}
        public override void VaciarHijos() {}

    }


    public class Familia_Patente {
        public int Id { get; set; }
        public int FamiliaId { get; set; }
        public FamiliaModel? Familia { get; set; }

        public int PatenteId { get; set; }
        public Patente? Patente { get; set; }
    }

    public class UserPermission {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public int PermisoId { get; set; }
    }




}