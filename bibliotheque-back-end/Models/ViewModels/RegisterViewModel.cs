using System.ComponentModel.DataAnnotations;

namespace BibliothequeBackEnd.Models.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Le nom est requis.")]
    [Display(Name = "Nom")]
    public string Nom { get; set; }

    [Required(ErrorMessage = "Le prénom est requis.")]
    [Display(Name = "Prénom")]
    public string Prenom { get; set; }

    [Required(ErrorMessage = "L'adresse email est requise.")]
    [EmailAddress(ErrorMessage = "Format d'adresse email invalide.")]
    [Display(Name = "Adresse email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Le mot de passe est requis.")]
    [DataType(DataType.Password)]
    [StringLength(100, ErrorMessage = "Le {0} doit contenir au moins {2} caractères.", MinimumLength = 6)]
    [Display(Name = "Mot de passe")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirmer le mot de passe")]
    [Compare("Password", ErrorMessage = "Le mot de passe et la confirmation ne correspondent pas.")]
    public string ConfirmPassword { get; set; }
}