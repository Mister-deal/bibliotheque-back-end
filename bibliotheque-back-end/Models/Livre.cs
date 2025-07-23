using System.Text.Json.Serialization;

namespace bibliotheque_back_end.Models
{
    public class Livre
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Categorie Categorie { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EtatLivre Etat { get; set; }

        public ICollection<EmpruntLivre> EmpruntLivres { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }
}
