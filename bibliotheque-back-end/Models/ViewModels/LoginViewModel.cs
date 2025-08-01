using System.ComponentModel.DataAnnotations;

namespace BibliothequeBackEnd.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "L'email ou le nom d'utilisateur est requis")]
        [Display(Name = "Email ou Nom d'utilisateur")]
        [StringLength(100, ErrorMessage = "L'email ou le nom d'utilisateur ne peut pas dépasser 100 caractères")]
        public string EmailOrUsername { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le mot de passe est requis")]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Le mot de passe doit contenir entre 6 et 100 caractères")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Se souvenir de moi")]
        public bool RememberMe { get; set; }
    }
}
