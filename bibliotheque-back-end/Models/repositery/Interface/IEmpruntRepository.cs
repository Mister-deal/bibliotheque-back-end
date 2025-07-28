namespace bibliotheque_back_end.Models.repositery;

public interface IEmpruntRepository
{
    IEnumerable<Emprunt> getAllEmprunts();
    Emprunt getEmpruntById(int id);
    void addEmprunt(Emprunt emprunt);
    void updateEmprunt(Emprunt emprunt);
    void deleteEmprunt(Emprunt emprunt);
    bool checkIfEmpruntExists(int id);
    
    Emprunt getEmpruntWithBooks(int id);
    IEnumerable<Emprunt> getEmpruntsContainingBook(int livreId);
}