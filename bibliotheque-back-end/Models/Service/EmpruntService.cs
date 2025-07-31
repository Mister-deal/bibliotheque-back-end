using bibliotheque_back_end.Data;
using bibliotheque_back_end.Models.DTO;
using bibliotheque_back_end.Models.repositery;
using bibliotheque_back_end.Models.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace bibliotheque_back_end.Models.Service;

public class EmpruntService : IEmpruntService
{
    private readonly IEmpruntRepository _empruntRepository;
    private readonly ILivreRepository _livreRepository;
    private readonly IMembreRepository _membreRepository;
    private readonly BibliothequeDb _context;
    private readonly IEmployeRepository _employeRepository;
    // ✅ SUPPRIMÉ : private IEmpruntService _empruntServiceImplementation; (inutile)

    public EmpruntService(IEmpruntRepository empruntRepository, ILivreRepository livreRepository,
        IMembreRepository membreRepository, BibliothequeDb context, IEmployeRepository employeRepository) // ✅ AJOUTÉ employeRepository
    {
        _empruntRepository = empruntRepository;
        _livreRepository = livreRepository;
        _membreRepository = membreRepository;
        _context = context;
        _employeRepository = employeRepository; // ✅ AJOUTÉ
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

        if (!await _membreRepository.CheckIfMemberExistsAsync(membreId))
        {
            throw new KeyNotFoundException($"Le membre avec l'ID {membreId} n'existe pas.");
        }
        var allEmprunts = await _empruntRepository.GetAllEmpruntsAsync();
        return allEmprunts.Where(e => e.MembreId == membreId); // ✅ CORRIGÉ : e.Id → e.MembreId
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
        return allEmprunts.Where(e => e.DateRetour == null); // ✅ CORRIGÉ : logique simplifiée
    }

    public async Task<Emprunt> CreateEmpruntAsync(EmpruntCreateDto  empruntCreateDto)
    {
       if (empruntCreateDto == null)
        {
            throw new ArgumentNullException(nameof(empruntCreateDto), "Emprunt creation data cannot be null.");
        }
        if (empruntCreateDto.MembreId <= 0 || empruntCreateDto.EmployeValidationId <= 0)
        {
            throw new ArgumentException("L'ID du membre et de l'employé doivent être supérieurs à zéro.");
        }
        if (empruntCreateDto.LivreIds == null || !empruntCreateDto.LivreIds.Any())
        {
            throw new ArgumentException("Au moins un ID de livre est requis pour créer un emprunt.", nameof(empruntCreateDto.LivreIds));
        }

        if (empruntCreateDto.DateRetour.HasValue && empruntCreateDto.DateRetour <= DateOnly.FromDateTime(DateTime.Now))
        {
            throw new ArgumentException("La date de retour prévue doit être future.", nameof(empruntCreateDto.DateRetour));
        }

        var membre = await _membreRepository.GetMemberAsync(empruntCreateDto.MembreId);
        if (membre == null)
        {
            throw new KeyNotFoundException($"Le membre avec l'ID {empruntCreateDto.MembreId} n'existe pas.");
        }

        var employe = await _employeRepository.GetEmployeeAsync(empruntCreateDto.EmployeValidationId);
        if (employe == null)
        {
            throw new KeyNotFoundException($"L'employé avec l'ID {empruntCreateDto.EmployeValidationId} n'existe pas.");
        }


        var livresEmpruntes = new List<EmpruntLivre>();
        foreach (var livreId in empruntCreateDto.LivreIds)
        {
            var livre = await _livreRepository.GetBookByIdAsync(livreId);
            if (livre == null)
            {
                throw new KeyNotFoundException($"Le livre avec l'ID {livreId} n'existe pas.");
            }
            if (livre.Etat != EtatLivre.Disponible)
            {
                throw new InvalidOperationException($"Le livre '{livre.Titre}' (ID: {livre.Id}) n'est pas disponible pour l'emprunt. Son état est : {livre.Etat}.");
            }
            livre.Etat = EtatLivre.Emprunte; // Marquer le livre comme emprunté
            await _livreRepository.UpdateBookAsync(livre); // Mettre à jour l'état du livre dans la DB

            livresEmpruntes.Add(new EmpruntLivre { IdLivre = livre.Id, livre = livre });
        }

        var newEmprunt = new Emprunt
        {
            MembreId = empruntCreateDto.MembreId,
            DateEmprunt = DateOnly.FromDateTime(DateTime.Now),
            DateRetour = empruntCreateDto.DateRetour,
            LivresEmpruntes = livresEmpruntes,
        };

        await _empruntRepository.AddEmpruntAsync(newEmprunt);
        await _context.SaveChangesAsync(); // Le SaveChanges est souvent au niveau du service ou de l'unité de travail.

        return newEmprunt;
    }

    public async Task<Emprunt?> ReturnSpecificBookFromEmpruntAsync(int empruntId, int livreId, int employeValidationId)
    {
        if (empruntId <= 0 || livreId <= 0 || employeValidationId <= 0)
        {
            throw new ArgumentException("Les IDs d'emprunt, de livre et d'employé doivent être supérieurs à zéro.");
        }

        // ✅ AJOUTÉ : Validation de l'employé
        var employe = await _employeRepository.GetEmployeeAsync(employeValidationId);
        if (employe == null)
        {
            throw new KeyNotFoundException($"L'employé avec l'ID {employeValidationId} n'existe pas.");
        }

        var emprunt = await _empruntRepository.GetEmpruntByIdAsync(empruntId);
        if (emprunt == null)
        {
            return null;
        }

        var empruntLivre = emprunt.LivresEmpruntes.FirstOrDefault(el => el.IdLivre == livreId);
        if (empruntLivre == null)
        {
            throw new InvalidOperationException(
                $"Le livre avec l'ID {livreId} n'appartient pas à l'emprunt {empruntId}.");
        }

        // ✅ AJOUTÉ : Marquer le livre comme disponible
        if (empruntLivre.livre != null && empruntLivre.livre.Etat != EtatLivre.Disponible)
        {
            empruntLivre.livre.Etat = EtatLivre.Disponible;
            await _livreRepository.UpdateBookAsync(empruntLivre.livre);
        }

        await _empruntRepository.UpdateEmpruntAsync(emprunt);
        await _context.SaveChangesAsync();

        return emprunt;
    }

    public async Task<Emprunt?> ReturnAllBooksForEmpruntAsync(int empruntId, int employeValidationId)
    {
        if (empruntId <= 0 || employeValidationId <= 0)
        {
            throw new ArgumentException("Les IDs d'emprunt et d'employé doivent être supérieurs à zéro.");
        }

        var employe = await _employeRepository.GetEmployeeAsync(employeValidationId);
        if (employe == null)
        {
            throw new ArgumentException($"L'employé avec l'ID {employeValidationId} n'existe pas.");
        }

        var emprunt = await _context.Emprunts
                                    .Include(e => e.LivresEmpruntes)
                                        .ThenInclude(el => el.livre)
                                    .FirstOrDefaultAsync(e => e.Id == empruntId);

        if (emprunt == null)
        {
            return null;
        }
        if (emprunt.DateRetour.HasValue)
        {
            throw new InvalidOperationException($"L'emprunt {empruntId} est déjà marqué comme entièrement retourné.");
        }

        bool atLeastOneNewBookReturned = false;

        foreach (var empruntLivre in emprunt.LivresEmpruntes)
        {
            if (empruntLivre.livre != null)
            {
                if (empruntLivre.livre.Etat != EtatLivre.Disponible)
                {
                    empruntLivre.livre.Etat = EtatLivre.Disponible;

                    await _livreRepository.UpdateBookAsync(empruntLivre.livre);
                    atLeastOneNewBookReturned = true;
                }
            }
            else
            {
                Console.WriteLine($"Avertissement: Livre associé à l'EmpruntLivre (ID: {empruntLivre.Id}) est introuvable lors du retour de tous les livres.");
            }
        }

        if (!atLeastOneNewBookReturned && emprunt.LivresEmpruntes.Any())
        {
            throw new InvalidOperationException($"Tous les livres de l'emprunt {empruntId} étaient déjà marqués comme disponibles.");
        }
        else if (!emprunt.LivresEmpruntes.Any())
        {
            throw new InvalidOperationException($"L'emprunt {empruntId} ne contient aucun livre.");
        }

        emprunt.DateRetour = DateOnly.FromDateTime(DateTime.UtcNow);

        await _context.SaveChangesAsync();

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
            return null;
        }

        await _empruntRepository.DeleteEmpruntAsync(existingEmprunt);
        await _context.SaveChangesAsync();

        return existingEmprunt;
    }

    public async Task<bool> EmpruntExistsAsync(int id)
    {
        return await _empruntRepository.CheckIfEmpruntExistsAsync(id);
    }

    // Partie Dashboard 
    public async Task<int> GetTodayLoansCountAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        return await _context.Emprunts
            .CountAsync(e => e.DateEmprunt == today);
    }

    public async Task<int> GetOverdueReturnsCountAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var maxLoanDays = 14;

        return await _context.Emprunts
            .CountAsync(e => e.DateRetour == null &&
                       e.DateEmprunt.AddDays(maxLoanDays) < today);
    }

    public async Task<IEnumerable<dynamic>> GetRecentActivitiesAsync()
    {
        return await _context.Emprunts
            .Include(e => e.Membre)
            .Include(e => e.LivresEmpruntes)
                .ThenInclude(el => el.livre)
            .OrderByDescending(e => e.DateEmprunt)
            .Take(10)
            .Select(e => new
            {
                Title = $"Emprunt par {e.Membre.Prenom} {e.Membre.Nom}",
                Description = $"{e.LivresEmpruntes.Count()} livre(s) emprunté(s)",
                Time = e.DateEmprunt.ToDateTime(TimeOnly.MinValue),
                Icon = "book"
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<dynamic>> GetPopularBooksAsync()
    {
        return await _context.EmpruntLivres
            .Include(el => el.livre)
            .GroupBy(el => new { el.livre.Id, el.livre.Titre, el.livre.Auteur })
            .Select(g => new
            {
                Title = g.Key.Titre,
                Author = g.Key.Auteur,
                BorrowCount = g.Count()
            })
            .OrderByDescending(x => x.BorrowCount)
            .Take(5)
            .ToListAsync();
    }
}
