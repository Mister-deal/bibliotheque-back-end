namespace bibliotheque_back_end.Models.repositery;

public interface IEmpruntRepository
{
    IEnumerable<Emprunt> GetAllEmprunts();
    Emprunt GetEmpruntById(int id);
    void AddEmprunt(Emprunt emprunt);
    void UpdateEmprunt(Emprunt emprunt);
    void DeleteEmprunt(Emprunt emprunt);
    bool CheckIfEmpruntExists(int id);
    
    Emprunt GetEmpruntWithBooks(int id);
    IEnumerable<Emprunt> GetEmpruntsContainingBook(int livreId);
}