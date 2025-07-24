using System.Text.Json.Serialization;

namespace bibliotheque_back_end.Models
{
    public class Employe
    {
        // -- Les propriétés de l'entité Employe --
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string MotDePasse { get; set; }
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Role Role { get; set; }
        public ICollection<Emprunt> empruntsValides { get; set; }
    }
}
