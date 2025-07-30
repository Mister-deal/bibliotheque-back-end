using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace bibliotheque_back_end.Models
{
    public class Emprunt
    {
        [SwaggerSchema("Id de l'emprunt")]
        public int Id { get; set; }

        // Clé étrangère vers Membre
        [Required(ErrorMessage = "L'ID du membre est requis.")]
        [SwaggerSchema("Id du membre qui emprunte")]
        public int MembreId { get; set; }

        // Navigation property vers Membre
        [ForeignKey("MembreId")]
        [SwaggerSchema("Membre qui fait l'emprunt")]
        public virtual Membre? Membre { get; set; }

        [Required(ErrorMessage = "La date d'emprunt est requise.")]
        [SwaggerSchema("Date de l'emprunt")]
        public DateOnly DateEmprunt { get; set; }

        [SwaggerSchema("Date de retour")]
        public DateOnly? DateRetour { get; set; }

        [SwaggerSchema("Liste des livres empruntés")]
        public virtual ICollection<EmpruntLivre> LivresEmpruntes { get; set; } = new List<EmpruntLivre>();

        // Constructeur vide (nécessaire pour Entity Framework)
        public Emprunt() { }

        // Constructeur avec paramètres incluant MembreId
        public Emprunt(int membreId, DateOnly dateEmprunt, DateOnly? dateRetour = null)
        {
            MembreId = membreId;
            DateEmprunt = dateEmprunt;
            DateRetour = dateRetour;
        }
    }
}
