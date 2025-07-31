using System.ComponentModel.DataAnnotations;

namespace BibliothequeBackEnd.Models.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "L'adresse email ou le nom d'utilisateur est requis.")]
    [Display(Name = "Adresse email ou Nom d'utilisateur")]
    public string EmailOrUsername { get; set; }

    [Required(ErrorMessage = "Le mot de passe est requis.")]
    [DataType(DataType.Password)]
    [Display(Name = "Mot de passe")]
    public string Password { get; set; }

    [Display(Name = "Se souvenir de moi")]
    public bool RememberMe { get; set; }
}