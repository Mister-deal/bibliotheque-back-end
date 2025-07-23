using System.ComponentModel.DataAnnotations;

namespace bibliotheque_back_end.Models.DTO
{
    public class EmpruntLivreDto
    {
        [Required(ErrorMessage = "L'identifiant de l'emprunt est requis.")]
        public int EmpruntId { get; set; }

        [Required(ErrorMessage = "L'identifiant du livre est requis.")]
        public int LivreId { get; set; }
    }
}
