using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace bibliotheque_back_end.Models
{
    public class Emprunt
    {
        [SwaggerSchema("Id de l'emprunt")]
        public int Id { get; set; }

        [Required(ErrorMessage = "La date d'emprunt est requise.")]
        [SwaggerSchema("Date de l'emprunt")]
        public DateOnly DateEmprunt { get; set; }

        [SwaggerSchema("Date de retour")]
        public DateOnly? DateRetour { get; set; }

        [SwaggerSchema("Liste des livres empruntés")]
        public ICollection<EmpruntLivre> LivresEmpruntes { get; set; } = new List<EmpruntLivre>();

        // Constructeur vide (nécessaire pour Entity Framework)
        public Emprunt() { }

        // Constructeur avec paramètres (optionnel)
        public Emprunt(DateOnly dateEmprunt, DateOnly? dateRetour = null)
        {
            DateEmprunt = dateEmprunt;
            DateRetour = dateRetour;
        }
    }
}
