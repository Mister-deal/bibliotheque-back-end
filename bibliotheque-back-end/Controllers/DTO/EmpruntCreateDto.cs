using System.ComponentModel.DataAnnotations;

namespace bibliotheque_back_end.Models.DTO;


    public class EmpruntCreateDto
    {
        [Required(ErrorMessage = "L'ID du membre est requis.")]
        public int MembreId { get; set; }

        [Required(ErrorMessage = "Au moins un ID de livre est requis.")]
        public List<int> LivreIds { get; set; } = new List<int>();

        public DateOnly? DateRetour{ get; set; }

        [Required(ErrorMessage = "L'ID de l'employé de validation est requis.")]
        public int EmployeValidationId { get; set; }
    }