namespace bibliotheque_back_end.Models
{
    public class Membre
    {
        // -- Les propriétés de l'entité Membre --
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public required string Email { get; set; }
        public required string MotDePasse { get; set; }
        
        public ICollection<Emprunt> emprunts { get; set; } =  new List<Emprunt>();
        public ICollection<Reservation> reservations { get; set; } =  new List<Reservation>();
    }
}
