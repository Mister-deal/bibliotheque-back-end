namespace bibliotheque_back_end.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        public int LivreId { get; set; }
        public Livre Livre { get; set; }

        public int MembreId { get; set; }
        public Membre Membre { get; set; }

        public DateOnly DateReservation { get; set; }
        public bool EstActive { get; set; } = true;
    }
}
