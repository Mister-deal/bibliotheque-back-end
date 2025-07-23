namespace bibliotheque_back_end.Models
{
    public class Emprunt
    {
        public ICollection<EmpruntLivre> LivresEmpruntes { get; set; }
    }
}
