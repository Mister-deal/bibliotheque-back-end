namespace bibliotheque_back_end.Models
{
    public class EmpruntLivre
    {
        public int Id { get; set; }
        public int? IdLivre { get; set; }
        public int? IdEmprunt { get; set; }
    }
}
