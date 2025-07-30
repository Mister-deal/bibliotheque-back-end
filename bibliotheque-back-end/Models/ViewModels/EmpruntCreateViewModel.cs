using System.ComponentModel.DataAnnotations;

namespace BibliothequeBackEnd.Models.ViewModels;

public class EmpruntCreateViewModel
{
    [Required(ErrorMessage = "L'ID du membre est requis.")]
    [Range(1, int.MaxValue, ErrorMessage = "L'ID du membre doit être positif.")]
    public int MembreId { get; set; }

    [Required(ErrorMessage = "Au moins un livre doit être sélectionné.")]
    [MinLength(1, ErrorMessage = "Au moins un livre doit être sélectionné.")]
    public List<int> LivreIds { get; set; } = new();

    [Display(Name = "Date de retour prévue")]
    public DateOnly? DateRetour{ get; set; }

    [Required(ErrorMessage = "L'ID de l'employé est requis pour validation.")]
    [Range(1, int.MaxValue, ErrorMessage = "L'ID de l'employé doit être positif.")]
    public int EmployeValidationId { get; set; }
}