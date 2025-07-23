namespace bibliotheque_back_end.Models
{
    public class Membre
    {
        // -- Les propriétés de l'entité Membre --
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string MotDePasse { get; set; }
        
        public ICollection<Emprunt> emprunts { get; set; }
        public ICollection<Reservation> reservations { get; set; }
    }
}
