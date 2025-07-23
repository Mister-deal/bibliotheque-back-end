using System.Text.Json.Serialization;

namespace bibliotheque_back_end.Models
{
    public class Livre
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Categorie Categorie { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EtatLivre Etat { get; set; }

        public int Id { get; set; }  // Identifiant unique du livre

        public required string Titre { get; set; }  // Titre du livre
        public required string Auteur { get; set; }  // Nom de l’auteur
        public string? Description { get; set; }  // Description du livre
        public required int AnneePublication { get; set; }  // Année de publication
        public required string Editeur { get; set; }  // Nom de l'éditeur


        public ICollection<EmpruntLivre> EmpruntLivres { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }
}
