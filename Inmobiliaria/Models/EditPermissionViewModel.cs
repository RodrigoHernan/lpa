namespace Inmobiliaria.Models
{
    public class EditPermissionViewModel
    {
        public string Id { get; set; }
        public List<Permiso> Permisos { get; set; }
        public List<Permiso> PermisosDisponibles { get; set; }
    }
}
