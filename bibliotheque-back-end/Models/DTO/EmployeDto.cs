namespace bibliotheque_back_end.Models.DTO
{
    public class EmployeDto
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string MotDePasse { get; set; }
        public Role Role { get; set; }
    }
}
