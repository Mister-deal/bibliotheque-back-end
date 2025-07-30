using System.ComponentModel.DataAnnotations;

namespace BibliothequeBackEnd.Models.ViewModels;

public class MembreEditViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Un nom de famille est requis.")]
    [StringLength(20, MinimumLength = 2, ErrorMessage = "Le nom doit contenir entre 2 et 20 caractères")]
    [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Le nom de famille doit contenir uniquement des lettres")]
    public string Nom { get; set; } = string.Empty;

    [Required(ErrorMessage = "Un prénom est requis.")]
    [StringLength(20, MinimumLength = 2, ErrorMessage = "Le prénom doit contenir entre 2 et 20 caractères")]
    [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Le prénom doit contenir uniquement des lettres")]
    public string Prenom { get; set; } = string.Empty;

    [Required(ErrorMessage = "Une adresse email est requise.")]
    [EmailAddress(ErrorMessage = "L'email fourni n'est pas valide.")]
    public string Email { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$",
        ErrorMessage = "Le nouveau mot de passe doit contenir au moins 8 caractères, une majuscule, une minuscule et un chiffre.")]
    [Display(Name = "Nouveau mot de passe")] 
    public string? NewMotDePasse { get; set; } 
}