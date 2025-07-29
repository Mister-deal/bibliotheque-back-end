using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace bibliotheque_back_end.Models
{
    public class Reservation
    {
        [SwaggerSchema("Id de la reservation")]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Vous devez sélectionner au moins un livre.")]
        [MinLength(1, ErrorMessage = "La réservation doit contenir au moins un livre.")]
        public int LivreId { get; set; }
        public Livre Livre { get; set; }

        [Required(ErrorMessage = "L'identifiant du membre est requis.")]
        public int MembreId { get; set; }
        public Membre Membre { get; set; }
        
        [Required(ErrorMessage = "La date de réservation est requise.")]
        public required DateOnly DateReservation { get; set; }
        public required bool EstActive { get; set; } = false;
    }
}
