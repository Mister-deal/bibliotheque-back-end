using System.ComponentModel.DataAnnotations;

namespace BibliothequeBackEnd.Models.ViewModels;

public class MembreCreateViewModel
{
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

    [Required(ErrorMessage = "Le mot de passe est requis.")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$",
        ErrorMessage = "Le mot de passe doit contenir au moins 8 caractères, une majuscule, une minuscule et un chiffre.")]
    public string MotDePasse { get; set; } = string.Empty;
}