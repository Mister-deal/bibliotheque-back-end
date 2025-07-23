using System.Collections.Generic;

namespace bibliotheque_back_end.Models
{
    public class Livre
    {
        public int Id { get; set; }  // Identifiant unique du livre

        public required string Titre { get; set; }  // Titre du livre
        public required string Auteur { get; set; }  // Nom de l’auteur
        public string? Description { get; set; }  // Description du livre
        public required int AnneePublication { get; set; }  // Année de publication
        public required string Editeur { get; set; }  // Nom de l'éditeur

        public Categorie Categorie { get; set; }  // Navigation vers la catégorie
        public EtatLivre Etat { get; set; }  // État du livre Enum disponible emprunté, réservé, endommagé, etc. 

        public ICollection<EmpruntLivre> EmpruntLivres { get; set; } = new List<EmpruntLivre>();
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
