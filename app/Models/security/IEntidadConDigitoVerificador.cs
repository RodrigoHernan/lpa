
namespace app.Models
{
    public interface IEntidadConDigitoVerificador
    {
        byte[] DVH { get; set; }
        Guid Id { get; set; }
    }

    public class ObjetosCorruptos
    {
        public Guid Id { get; set; }
        public string NombreTabla { get; set; }

        public string Accion { get; set; }

    }
}