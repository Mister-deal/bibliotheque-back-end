using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace bibliotheque_back_end.Models
{
    public class Employe
    {
        [SwaggerSchema("Id de l'employé")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Un nom de famille est requis.")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Le nom doit contenir entre 2 et 20 caractères")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Le nom de famille doit contenir uniquement des lettres")]
        [SwaggerSchema("Nom de famille de l'employé")]
        public string Nom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Un prénom est requis.")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Le prénom doit contenir entre 2 et 20 caractères")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Le prénom doit contenir uniquement des lettres")]
        [SwaggerSchema("Prénom de l'employé")]
        public string Prenom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Une adresse email est requise.")]
        [EmailAddress(ErrorMessage = "L'email fourni n'est pas valide.")]
        [SwaggerSchema("Adresse email de l'employé")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le mot de passe est requis.")]
        [StringLength(255)]
        [SwaggerSchema("Mot de passe de l'employé")]
        public string MotDePasse { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le rôle est requis.")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [SwaggerSchema("Rôle de l'employé")]
        public Role Role { get; set; } = Role.Bibliothecaire;

        [SwaggerSchema("Liste des emprunts valides de l'employé")]
        public ICollection<Emprunt> EmpruntsValides { get; set; } = new List<Emprunt>();

        // Constructeur vide (nécessaire pour Entity Framework)
        public Employe() { }
    }
}
