using bibliotheque_back_end.Data;
using bibliotheque_back_end.Models.repositery;
using bibliotheque_back_end.Models.Service.Interface;

namespace bibliotheque_back_end.Models.Service;

public class EmpruntService : IEmpruntService
{
    private readonly IEmpruntRepository _empruntRepository;
    private readonly ILivreRepository _livreRepository;
    private readonly IMembreRepository _membreRepository;
    private readonly BibliothequeDb _context; // Ajout du DbContext pour SaveChangesAsync()

    public EmpruntService(IEmpruntRepository empruntRepository, ILivreRepository livreRepository,
        IMembreRepository membreRepository, BibliothequeDb context)
    {
        _empruntRepository = empruntRepository;
        _livreRepository = livreRepository;
        _membreRepository = membreRepository;
        _context = context; // Initialisation du DbContext
    }

    public async Task<IEnumerable<Emprunt>> GetAllEmpruntsAsync()
    {
        return await _empruntRepository.GetAllEmpruntsAsync();
    }

    public async Task<IEnumerable<Emprunt>> GetEmpruntsByMembreIdAsync(int membreId)
    {
        // Exemple d'implémentation : vous devrez peut-être ajouter une méthode spécifique dans IEmpruntRepository
        // ou filtrer ici après avoir récupéré tous les emprunts.
        if (membreId <= 0)
        {
            throw new ArgumentException("L'ID du membre doit être supérieur à zéro.", nameof(membreId));
        }

        // Vérifiez si le membre existe si nécessaire
        if (!await _membreRepository.CheckIfMemberExistsAsync(membreId))
        {
            throw new KeyNotFoundException($"Le membre avec l'ID {membreId} n'existe pas.");
        }

        // Si le repository a une méthode spécifique, utilisez-la :
        // return await _empruntRepository.GetEmpruntsByMembreIdAsync(membreId);

        // Sinon, filtrez la collection complète (moins efficace pour de grandes bases) :
        var allEmprunts = await _empruntRepository.GetAllEmpruntsAsync();
        return allEmprunts.Where(e => e.Id == membreId);
        // Ou mieux, ajoutez une méthode GetEmpruntsByMembreIdAsync(int membreId) à votre IEmpruntRepository
        // et son implémentation dans EmpruntRepository pour un filtrage côté DB.
    }

    public async Task<IEnumerable<Emprunt>> GetEmpruntsByLivreIdAsync(int livreId)
    {
        if (livreId <= 0)
        {
            throw new ArgumentException("L'ID du livre doit être supérieur à zéro.", nameof(livreId));
        }

        // Vérifiez si le livre existe si nécessaire
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
        // Le retour null est géré par l'interface IEmpruntService
        return emprunt;
    }

    public async Task<IEnumerable<Emprunt>> GetActiveEmpruntsAsync()
    {
        // Il faudrait ajouter une propriété "DateRetourEffective" ou "EstActif" à Emprunt
        // et/ou une méthode spécifique dans le repository pour une logique robuste.
        var allEmprunts = await _empruntRepository.GetAllEmpruntsAsync();
        // Exemple de logique simple (à adapter selon votre modèle d'affaires) :
        // Considérer un emprunt actif s'il n'a pas de date de retour effective et que sa date de retour prévue n'est pas passée
        return allEmprunts.Where(e => e.DateRetour > DateOnly.FromDateTime(DateTime.Now) && e.DateRetour == null);
        // Si vous avez un champ 'IsActive' dans votre modèle, ce serait plus simple :
        // return allEmprunts.Where(e => e.IsActive);
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

        // Vous devriez vérifier que l'employé existe aussi
        // var employe = await _employeRepository.GetEmployeeAsync(employeValidationId);
        // if (employe == null) { throw new KeyNotFoundException($"L'employé avec l'ID {employeValidationId} n'existe pas."); }

        var livresEmpruntes = new List<EmpruntLivre>();
        foreach (var livreId in livreIds)
        {
            var livre = await _livreRepository.GetBookByIdAsync(livreId);
            if (livre == null)
            {
                throw new KeyNotFoundException($"Le livre avec l'ID {livreId} n'existe pas.");
            }
            // Logique pour vérifier si le livre est disponible à l'emprunt
            // Par exemple, vérifier s'il n'est pas déjà emprunté ou réservé
            // if (!livre.IsAvailable) { throw new InvalidOperationException($"Le livre '{livre.Titre}' n'est pas disponible pour l'emprunt."); }

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

        // Vérifier si le livre est bien dans cet emprunt et s'il n'a pas déjà été retourné
        var empruntLivre = emprunt.LivresEmpruntes.FirstOrDefault(el => el.IdLivre == livreId);
        if (empruntLivre == null)
        {
            throw new InvalidOperationException(
                $"Le livre avec l'ID {livreId} n'appartient pas à l'emprunt {empruntId}.");
        }
        // Il faudrait une propriété pour marquer un livre comme retourné dans EmpruntLivre
        // Par exemple : if (empruntLivre.EstRetourne) { throw new InvalidOperationException("Ce livre a déjà été retourné pour cet emprunt."); }

        // Mettre à jour l'état du livre dans l'emprunt (ex: empruntLivre.EstRetourne = true;)
        // Si tous les livres sont retournés, marquer l'emprunt comme terminé
        // if (emprunt.LivresEmpruntes.All(el => el.EstRetourne))
        // {
        //    emprunt.DateRetourEffective = DateOnly.FromDateTime(DateTime.Now);
        // }

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

        // Logique métier : peut-être ne pas autoriser la suppression si l'emprunt est actif ou contient des livres non rendus.
        // if (existingEmprunt.DateRetourEffective == null && existingEmprunt.DateRetour > DateOnly.FromDateTime(DateTime.Now))
        // {
        //     throw new InvalidOperationException("Impossible de supprimer un emprunt actif. Veuillez d'abord retourner tous les livres.");
        // }

        await _empruntRepository.DeleteEmpruntAsync(existingEmprunt);
        await _context.SaveChangesAsync(); // Sauvegarde asynchrone des changements

        return existingEmprunt; // Retourne l'emprunt qui a été supprimé
    }

    public async Task<bool> EmpruntExistsAsync(int id)
    {
        return await _empruntRepository.CheckIfEmpruntExistsAsync(id);
    }
}