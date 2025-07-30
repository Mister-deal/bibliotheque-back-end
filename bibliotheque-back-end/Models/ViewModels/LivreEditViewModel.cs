using System.ComponentModel.DataAnnotations;
using bibliotheque_back_end.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace BibliothequeBackEnd.Models.ViewModels;

public class LivreEditViewModel
    {
        [SwaggerSchema("Identifiant unique du livre (clé primaire)")]
        public int Id { get; set; } 

        [Required(ErrorMessage = "Un titre est requis.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Le titre doit contenir entre 1 et 100 caractères.")]
        [RegularExpression(@"^[A-Za-zÀ-ÿ0-9\s\-\.',:!?()«»”“\""&/]+$", ErrorMessage = "Le titre contient des caractères non autorisés.")]
        [SwaggerSchema("Titre du livre")]
        public string Titre { get; set; }

        [Required(ErrorMessage = "Un nom d'auteur est requis.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "L'auteur doit contenir entre 2 et 50 caractères.")]
        [RegularExpression(@"^[A-Za-zÀ-ÿ\s\-'’]+$", ErrorMessage = "Le nom de l'auteur ne doit contenir que des lettres, espaces, tirets ou apostrophes.")]
        [SwaggerSchema("Nom et prénom complet de l’auteur")]
        public string Auteur { get; set; }

        [StringLength(500, ErrorMessage = "La description doit contenir maximum 500 caractères.")]
        [SwaggerSchema("Description du livre")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Une année de publication est requise.")]
        [Range(1000, 2100, ErrorMessage = "L'année doit être comprise entre 1000 et 2100.")]
        [SwaggerSchema("Année de publication du livre")]
        public int AnneePublication { get; set; }

        [Required(ErrorMessage = "Un éditeur est requis.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "L'éditeur doit contenir entre 1 et 50 caractères.")]
        [RegularExpression(@"^[A-Za-zÀ-ÿ0-9\s\-\.',:!?()«»”“""]+$", ErrorMessage = "L'éditeur contient des caractères non autorisés.")]
        [SwaggerSchema("Nom de l'éditeur")]
        public string Editeur { get; set; } 

        [Required(ErrorMessage = "La catégorie est requise.")]
        [SwaggerSchema("Catégorie du livre (ex: Roman, Essai, BD...)")]
        public Categorie Categorie { get; set; }
    }