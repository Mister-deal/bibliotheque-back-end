namespace bibliotheque_back_end.Models
{
    public class Membre
    {
        public ICollection<Emprunt> emprunts { get; set; }
        public ICollection<Reservation> reservations { get; set; }
    }
}
