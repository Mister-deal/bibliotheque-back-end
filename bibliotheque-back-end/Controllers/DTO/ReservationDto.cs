using System.ComponentModel.DataAnnotations;

namespace bibliotheque_back_end.Models.DTO
{
    public class ReservationDto
    {
        public int Id { get; set; } // Utilisé uniquement pour la lecture

        [Required(ErrorMessage = "L'identifiant du membre est requis.")]
        public int MembreId { get; set; }

        [Required(ErrorMessage = "Vous devez sélectionner au moins un livre.")]
        [MinLength(1, ErrorMessage = "La réservation doit contenir au moins un livre.")]
        public List<int> LivreIds { get; set; } = new();

        [Required(ErrorMessage = "La date de réservation est requise.")]
        public DateOnly DateReservation { get; set; }

        public bool EstValidee { get; set; } = false; // Peut être ignoré côté client lors de la création
    }
}
