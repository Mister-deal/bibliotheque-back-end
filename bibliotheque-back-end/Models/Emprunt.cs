namespace bibliotheque_back_end.Models
{
    public class Emprunt
    {
        // -- Les propriétés de l'entité Emprunt --
        public int Id { get; set; }
        public DateOnly DateEmprunt { get; set; }
        public DateOnly DateRetour { get; set; }

        public ICollection<EmpruntLivre> LivresEmpruntes { get; set; } = new List<EmpruntLivre>();
    }
}
