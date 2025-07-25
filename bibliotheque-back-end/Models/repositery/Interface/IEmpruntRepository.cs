namespace bibliotheque_back_end.Models.repositery;

public interface IEmpruntRepository
{
    IEnumerable<Emprunt> getAllEmprunts();
    Emprunt getEmpruntByEmail(string email);
    void addEmprunt(Emprunt emprunt);
    void updateEmprunt(Emprunt emprunt);
    void deleteEmprunt(Emprunt emprunt);
    bool checkIfEmpruntExists(string email);
}