namespace bibliotheque_back_end.Models
{
    public class Livre
    {
        public Categorie Categorie { get; set; }
        public EtatLivre Etat { get; set; }

        public ICollection<EmpruntLivre> EmpruntLivres { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }
}
