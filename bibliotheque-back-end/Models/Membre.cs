using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace bibliotheque_back_end.Models
{
    public class Membre
    {
        // -- Les propriétés de l'entité Membre --
        [SwaggerSchema("Id du membre")]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Un nom de famille est requis.")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Le nom doit contenir entre 2 et 20 caractères")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Le nom de famille doit contenir uniquement des lettres")]
        [SwaggerSchema("Nom de famille du membre")]
        public string Nom { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Un prénom est requis.")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Le prénom doit contenir entre 2 et 20 caractères")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Le prénom doit contenir uniquement des lettres")]
        [SwaggerSchema("Prenom du membre")]
        public string Prenom { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Une adresse email est requise.")]
        [EmailAddress(ErrorMessage = "L'email fourni n'est pas valide.")]
        [SwaggerSchema("Adresse email du membre")]
        public required string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Le mot de passe est requis.")]
        [StringLength(255)]
        [SwaggerSchema("Mot de passe haché de l'employé")]
        public required string MotDePasse { get; set; } = string.Empty;
        
        // - Documentation Mot de Passe : 
        //   ^            # Début de chaîne
        //   (?=.*[a-z])  # Au moins une lettre minuscule
        //   (?=.*[A-Z])  # Au moins une lettre majuscule
        //   (?=.*\d)     # Au moins un chiffre
        //   .{8,}        # Au moins 8 caractères
        //   $            # Fin de chaîne
        
        public ICollection<Emprunt> emprunts { get; set; } =  new List<Emprunt>();
        public ICollection<Reservation> reservations { get; set; } =  new List<Reservation>();
        
    }
}
