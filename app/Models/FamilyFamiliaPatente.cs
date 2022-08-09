namespace app.Models
{
    public class FamilyFamiliaPatenteModel
    {
        public FamiliaModel Familia { get; set; }
        public List<Familia_Patente> FamiliasPatentes { get; set; }
        public List<Patente> PatentesDisponibles { get; set; }
    }
}


