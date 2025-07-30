namespace bibliotheque_back_end.Models.repositery;

public interface IEmpruntRepository
{
    Task<IEnumerable<Emprunt>> GetAllEmpruntsAsync();
    Task<Emprunt?> GetEmpruntByIdAsync(int id);
    Task AddEmpruntAsync(Emprunt emprunt);
    Task UpdateEmpruntAsync(Emprunt emprunt);
    Task DeleteEmpruntAsync(Emprunt emprunt);
    Task<bool> CheckIfEmpruntExistsAsync(int id);
    
    Task<Emprunt?> GetEmpruntWithBooksAsync(int id); // Pour inclure les entités liées
    Task<IEnumerable<Emprunt>> GetEmpruntsContainingBookAsync(int livreId);
    Task<IEnumerable<Emprunt>> GetEmpruntsByMembreIdAsync(int membreId); 
}