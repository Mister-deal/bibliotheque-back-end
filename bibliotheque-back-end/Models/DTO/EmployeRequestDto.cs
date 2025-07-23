using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace bibliotheque_back_end.Models.DTO
{
    public class EmployeRequestDto
    {
        //permet de gérer la création (POST) et la mise à jour (PUT) d'une requête
        [Required(ErrorMessage = "Le nom de l'employe est obligatoire.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Le nom doit faire entre 3 et 20 caractères")]
        public string Nom { get; set; } = string.Empty;
        [Required(ErrorMessage = "Le prenom de l'employe est obligatoire.")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Le prenom doit faire entre 2 et 20 caractères")]
        public string Prenom { get; set; } = string.Empty;
        [Required(ErrorMessage = "Le mail de l'employe est obligatoire.")]
        [EmailAddress(ErrorMessage = "format mail invalide")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Le mot de passe est requis.")]
        [MinLength(8, ErrorMessage = "Le mot de passe doit contenir au moins 8 caractères.")]
        public string MotDePasse { get; set; } = string.Empty;
        [Required(ErrorMessage = "Le rôle est requis.")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Role Role { get; set; } = Role.Bibliothecaire;
    }
}
