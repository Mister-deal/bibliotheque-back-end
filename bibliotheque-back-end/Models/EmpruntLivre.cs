namespace bibliotheque_back_end.Models
{
    //entité d'association entre Entité Emprunt et Entité Livre
    public class EmpruntLivre
    {
        public int Id { get; set; }
        public int? IdLivre { get; set; }
        public Livre livre { get; set; }
        public int? IdEmprunt { get; set; }
        public Emprunt emprunt { get; set; }
    }
}
