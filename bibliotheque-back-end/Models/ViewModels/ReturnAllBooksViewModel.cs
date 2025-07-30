using System.ComponentModel.DataAnnotations;

namespace BibliothequeBackEnd.Models.ViewModels;

public class ReturnAllBooksViewModel
{
    [Required]
    public int EmpruntId { get; set; }

    [Display(Name = "Détails de l'emprunt")]
    public string? EmpruntDetails { get; set; } // For display purposes on the confirmation page

    [Required(ErrorMessage = "L'ID de l'employé est requis pour valider le retour.")]
    [Range(1, int.MaxValue, ErrorMessage = "L'ID de l'employé doit être positif.")]
    public int EmployeValidationId { get; set; }
}