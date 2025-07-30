using bibliotheque_back_end.Data;
using bibliotheque_back_end.Models.repositery;
using bibliotheque_back_end.Models.Service.Interface;

namespace bibliotheque_back_end.Models.Service;

public class EmpruntService : IEmpruntService
{
    private readonly IEmpruntRepository _empruntRepository;
    private readonly ILivreRepository _livreRepository;
    private readonly IMembreRepository _membreRepository;
    private readonly BibliothequeDb _context;

    public EmpruntService(IEmpruntRepository empruntRepository, ILivreRepository livreRepository,
        IMembreRepository membreRepository, BibliothequeDb context)
    {
        _empruntRepository = empruntRepository;
        _livreRepository = livreRepository;
        _membreRepository = membreRepository;
        _context = context;
    }

    public async Task<IEnumerable<Emprunt>> GetAllEmpruntsAsync()
    {
        return await _empruntRepository.GetAllEmpruntsAsync();
    }

    public async Task<IEnumerable<Emprunt>> GetEmpruntsByMembreIdAsync(int membreId)
    {
        if (membreId <= 0)
        {
            throw new ArgumentException("L'ID du membre doit être supérieur à zéro.", nameof(membreId));
        }

        // Vérifiez si le membre existe si nécessaire
        if (!await _membreRepository.CheckIfMemberExistsAsync(membreId))
        {
            throw new KeyNotFoundException($"Le membre avec l'ID {membreId} n'existe pas.");
        }
        var allEmprunts = await _empruntRepository.GetAllEmpruntsAsync();
        return allEmprunts.Where(e => e.Id == membreId);
    }

    public async Task<IEnumerable<Emprunt>> GetEmpruntsByLivreIdAsync(int livreId)
    {
        if (livreId <= 0)
        {
            throw new ArgumentException("L'ID du livre doit être supérieur à zéro.", nameof(livreId));
        }

        if (!await _livreRepository.BookExistsAsync(livreId))
        {
            throw new KeyNotFoundException($"Le livre avec l'ID {livreId} n'existe pas.");
        }

        return await _empruntRepository.GetEmpruntsContainingBookAsync(livreId);
    }

    public async Task<Emprunt?> GetEmpruntByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("L'ID de l'emprunt doit être supérieur à zéro.", nameof(id));
        }

        var emprunt = await _empruntRepository.GetEmpruntByIdAsync(id);
        return emprunt;
    }

    public async Task<IEnumerable<Emprunt>> GetActiveEmpruntsAsync()
    {
        var allEmprunts = await _empruntRepository.GetAllEmpruntsAsync();
        return allEmprunts.Where(e => e.DateRetour > DateOnly.FromDateTime(DateTime.Now) && e.DateRetour == null);
    }

    public async Task<Emprunt> CreateEmpruntAsync(int membreId, List<int> livreIds, DateOnly? dateRetour,
        int employeValidationId)
    {
        if (membreId <= 0 || employeValidationId <= 0)
        {
            throw new ArgumentException("L'ID du membre et de l'employé doivent être supérieurs à zéro.");
        }

        if (livreIds == null || !livreIds.Any())
        {
            throw new ArgumentException("Au moins un ID de livre est requis pour créer un emprunt.", nameof(livreIds));
        }

        if (dateRetour <= DateOnly.FromDateTime(DateTime.Now))
        {
            throw new ArgumentException("La date de retour doit être future.", nameof(dateRetour));
        }

        var membre = await _membreRepository.GetMemberAsync(membreId);
        if (membre == null)
        {
            throw new KeyNotFoundException($"Le membre avec l'ID {membreId} n'existe pas.");
        }


        var livresEmpruntes = new List<EmpruntLivre>();
        foreach (var livreId in livreIds)
        {
            var livre = await _livreRepository.GetBookByIdAsync(livreId);
            if (livre == null)
            {
                throw new KeyNotFoundException($"Le livre avec l'ID {livreId} n'existe pas.");
            }
            
            livresEmpruntes.Add(new EmpruntLivre { IdLivre = livre.Id, livre = livre });
        }

        var newEmprunt = new Emprunt
        {
            Id = membreId,
            DateEmprunt = DateOnly.FromDateTime(DateTime.Now),
            DateRetour = dateRetour,
            LivresEmpruntes = livresEmpruntes
        };

        await _empruntRepository.AddEmpruntAsync(newEmprunt);
        await _context.SaveChangesAsync(); // Sauvegarde asynchrone des changements

        return newEmprunt;
    }

    public async Task<Emprunt?> ReturnSpecificBookFromEmpruntAsync(int empruntId, int livreId, int employeValidationId)
    {
        if (empruntId <= 0 || livreId <= 0 || employeValidationId <= 0)
        {
            throw new ArgumentException("Les IDs d'emprunt, de livre et d'employé doivent être supérieurs à zéro.");
        }

        var emprunt = await _empruntRepository.GetEmpruntByIdAsync(empruntId);
        if (emprunt == null)
        {
            return null; // Emprunt non trouvé
        }
        var empruntLivre = emprunt.LivresEmpruntes.FirstOrDefault(el => el.IdLivre == livreId);
        if (empruntLivre == null)
        {
            throw new InvalidOperationException(
                $"Le livre avec l'ID {livreId} n'appartient pas à l'emprunt {empruntId}.");
        }

        await _empruntRepository.UpdateEmpruntAsync(emprunt);
        await _context.SaveChangesAsync(); // Sauvegarde asynchrone

        return emprunt;
    }

    public async Task<Emprunt?> DeleteEmpruntAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("L'ID de l'emprunt doit être supérieur à zéro.", nameof(id));
        }

        var existingEmprunt = await _empruntRepository.GetEmpruntByIdAsync(id);
        if (existingEmprunt == null)
        {
            return null; // Emprunt non trouvé
        }
        

        await _empruntRepository.DeleteEmpruntAsync(existingEmprunt);
        await _context.SaveChangesAsync(); // Sauvegarde asynchrone des changements

        return existingEmprunt; // Retourne l'emprunt qui a été supprimé
    }

    public async Task<bool> EmpruntExistsAsync(int id)
    {
        return await _empruntRepository.CheckIfEmpruntExistsAsync(id);
    }
}