using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace bibliotheque_back_end.Models
{
    public class Livre
    {
        [Required(ErrorMessage = "La catégorie est requise.")]
        [SwaggerSchema("Catégorie du livre (ex. : Roman, Essai, BD, etc.)")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Categorie Categorie { get; set; }
        
        
        [Required(ErrorMessage = "L'état du livre est requis.")]
        [SwaggerSchema("État physique du livre (ex. : Neuf, Bon, Usagé)")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EtatLivre Etat { get; set; } = EtatLivre.Disponible;

        
        [SwaggerSchema("Id de l'ouvrage")]
        public int Id { get; set; }  // Identifiant unique du livre
        
        
        [Required(ErrorMessage = "Un titre est requis.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Le titre doit contenir entre 1 et 100 caractères")]
        [RegularExpression(@"^[A-Za-zÀ-ÿ0-9\s\-\.',:!?()«»”“\""&/]+$",
            ErrorMessage = "Le titre contient des caractères non autorisés.")]
        [SwaggerSchema("Titre du livre")]
        public required string Titre { get; set; }  // Titre du livre
        
        
        [StringLength(50, MinimumLength = 2, ErrorMessage = "L'auteur doit contenir entre 2 et 50 caractères")]
        [RegularExpression(@"^[A-Za-zÀ-ÿ\s\-'’]+$", ErrorMessage = "Le nom de l'auteur ne doit contenir que des lettres, espaces, tirets ou apostrophes.")]
        [SwaggerSchema("Nom et prenom complet de l’auteur")]
        public string? Auteur { get; set; }  // Nom de l’auteur
        [StringLength(500, ErrorMessage = "La description doit contenir maximum 500 caractères")]
        [SwaggerSchema("Description de l'ouvrage")]
        
        public string? Description { get; set; }  // Description du livre
        [Required(ErrorMessage = "Une année de publication est requise.")]
        [Range(1000, 2100, ErrorMessage = "L'année doit être comprise entre 1000 et 2100.")]
        [SwaggerSchema("Année de publication de l'ouvrage")]
        
        public required int AnneePublication { get; set; }  // Année de publication
        [Required(ErrorMessage = "Un éditeur est requis.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "L'éditeur doit contenir entre 1 et 50 caractères")]
        [RegularExpression(@"^[A-Za-zÀ-ÿ0-9\s\-\.',:!?()«»”“""]+$",
            ErrorMessage = "L'éditeur contient des caractères non autorisés.")]
        [SwaggerSchema("Editeur")]
        public required string Editeur { get; set; }  // Nom de l'éditeur


        public ICollection<EmpruntLivre> EmpruntLivres { get; set; } =  new List<EmpruntLivre>();
        public ICollection<Reservation> Reservations { get; set; } =  new List<Reservation>();
    }
}
