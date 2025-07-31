using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace bibliotheque_back_end.Models.DTO;

[SwaggerSchema("Modèle pour les requêtes de connexion (Login)")]
public class LoginRequest
{
    [Required(ErrorMessage = "L'email est requis.")]
    [EmailAddress(ErrorMessage = "Le format de l'email est invalide.")]
    [SwaggerSchema("Adresse email de l'utilisateur")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le mot de passe est requis.")]
    [SwaggerSchema("Mot de passe en clair de l'utilisateur")]
    public string MotDePasse { get; set; } = string.Empty; // C'est le mot de passe en clair qui sera haché pour vérification
}