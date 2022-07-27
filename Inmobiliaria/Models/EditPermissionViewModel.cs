namespace Inmobiliaria.Models
{
    public class EditPermissionViewModel
    {
        public string Id { get; set; }
        public List<PermisoModel> Permisos { get; set; }
        public List<PermisoModel> PermisosDisponibles { get; set; }
    }
}
