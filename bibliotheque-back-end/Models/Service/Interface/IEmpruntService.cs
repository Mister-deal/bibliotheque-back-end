namespace bibliotheque_back_end.Models.Service.Interface;

public interface IEmpruntService
{
    IEnumerable<Emprunt> GetAllEmprunts();
    IEnumerable<Emprunt> GetEmpruntsByMembreId(int membreId);
    IEnumerable<Emprunt> GetEmpruntsByLivreId(int livreId);
    Emprunt GetEmpruntById(int id);
    IEnumerable<Emprunt> GetActiveEmprunts();
    
    Emprunt CreateEmprunt(int membreId, List<int> livreIds, DateOnly dateRetour, int employeValidationId);
    Emprunt ReturnSpecificBookFromEmprunt(int empruntId, int livreId, int employeValidationId); // Retourne un livre spécifique d'un emprunt
    Emprunt DeleteEmprunt(int id);
    bool EmpruntExists(int id);
}