using System.ComponentModel.DataAnnotations;
using bibliotheque_back_end.Models;

namespace BibliothequeBackEnd.Models.ViewModels;

public class EmployeEditViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Le nom est requis.")]
    [StringLength(50, ErrorMessage = "Le nom ne peut pas dépasser 50 caractères.")]
    public string Nom { get; set; }

    [Required(ErrorMessage = "Le prénom est requis.")]
    [StringLength(50, ErrorMessage = "Le prénom ne peut pas dépasser 50 caractères.")]
    public string Prenom { get; set; }

    [Required(ErrorMessage = "L'email est requis.")]
    [EmailAddress(ErrorMessage = "Le format de l'email est invalide.")]
    [StringLength(100, ErrorMessage = "L'email ne peut pas dépasser 100 caractères.")]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Le mot de passe doit contenir entre 6 et 100 caractères.")] 
    [Display(Name = "Nouveau mot de passe")]
    public string? NewMotDePasse { get; set; }

    [Required(ErrorMessage = "Le rôle est requis.")]
    public Role Role { get; set; }
}