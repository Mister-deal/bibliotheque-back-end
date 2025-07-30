using bibliotheque_back_end.Data;
using bibliotheque_back_end.Models.repositery;
using bibliotheque_back_end.Models.Service.Interface;

namespace bibliotheque_back_end.Models.Service;

public class MembreService : IMembreService
{
    private readonly IMembreRepository _membreRepository;
    private readonly IEmpruntRepository _empruntRepository;
    private readonly BibliothequeDb _context;

    public MembreService(IMembreRepository membreRepository, IEmpruntRepository empruntRepository, BibliothequeDb context)
    {
        _membreRepository = membreRepository;
        _empruntRepository = empruntRepository;
        _context = context;
    }

    public async Task<IEnumerable<Membre>> GetAllMembersAsync()
    {
        return await _membreRepository.GetAllMembersAsync();
    }

    public async Task<Membre?> GetMemberByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("L'ID du membre doit être supérieur à zéro.", nameof(id));
        }

        var member = await _membreRepository.GetMemberAsync(id);
        return member;
    }

    public async Task<Membre?> GetMemberByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("L'email ne peut pas être nul ou vide.", nameof(email));
        }
        return await _membreRepository.GetMemberByEmailAsync(email);
    }

    public async Task<Membre> AddMemberAsync(Membre newMember, string dataPassword)
    {
        if (newMember == null)
        {
            throw new ArgumentNullException(nameof(newMember), "L'objet membre ne peut pas être nul.");
        }

        if (string.IsNullOrWhiteSpace(dataPassword))
        {
            throw new ArgumentException("Le mot de passe ne peut être nul ou vide.", nameof(dataPassword));
        }
        if (string.IsNullOrWhiteSpace(newMember.Email) ||
            string.IsNullOrWhiteSpace(newMember.Nom) ||
            string.IsNullOrWhiteSpace(newMember.Prenom))
        {
            throw new ArgumentException("Email, nom et prénom sont nécessaires pour la création d'un nouveau membre.", nameof(newMember));
        }
        var allMembers = await _membreRepository.GetAllMembersAsync();
        if (allMembers.Any(m => m.Email.Equals(newMember.Email, StringComparison.OrdinalIgnoreCase)))
        {
             throw new InvalidOperationException($"Un membre avec le même email '{newMember.Email}' existe déjà.");
        }
        
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(dataPassword);
        newMember.MotDePasse = passwordHash;
        
        await _membreRepository.AddMemberAsync(newMember);
        await _context.SaveChangesAsync(); // Sauvegarde asynchrone des changements

        return newMember;
    }

     public async Task<Membre?> UpdateMemberAsync(int id, Membre updatedMember)
        {
            if (updatedMember == null)
            {
                throw new ArgumentNullException(nameof(updatedMember), "Les données du membre mis à jour ne peuvent être nulles.");
            }
            if (id <= 0)
            {
                throw new ArgumentException("L'ID du membre doit être supérieur à zéro.", nameof(id));
            }
            if (id != updatedMember.Id)
            {
                throw new ArgumentException("L'ID de l'URL ne correspond pas à l'ID du membre fourni.", nameof(id));
            }
            
            var existingMember = await _membreRepository.GetMemberAsync(id);
            if (existingMember == null)
            {
                return null;
            }
            
            existingMember.Nom = updatedMember.Nom;
            existingMember.Email = updatedMember.Email;
            existingMember.Prenom = updatedMember.Prenom;
            
            if (!string.IsNullOrWhiteSpace(updatedMember.MotDePasse))
            {
                existingMember.MotDePasse = BCrypt.Net.BCrypt.HashPassword(updatedMember.MotDePasse);
            }

            await _membreRepository.UpdateMemberAsync(existingMember);
            await _context.SaveChangesAsync();

            return existingMember;
        }
     

    public async Task<Membre?> DeleteMemberAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("L'ID du membre doit être supérieur à zéro.", nameof(id));
        }

        var existingMember = await _membreRepository.GetMemberAsync(id);
        if (existingMember == null)
        {
            return null; // Retourne null si le membre n'existe pas.
        }
        
        var activeLoans = await _empruntRepository.GetEmpruntsByMembreIdAsync(id);
        if (activeLoans.Any(e => e.DateRetour == null)) // Vérifie les emprunts non encore retournés
        {
            throw new InvalidOperationException($"Impossible de supprimer le membre avec l'ID {id} car il a des emprunts actifs en cours.");
        }

        await _membreRepository.DeleteMemberAsync(existingMember);
        await _context.SaveChangesAsync(); // Sauvegarde asynchrone des changements

        return existingMember; // Retourne le membre qui a été supprimé
    }

    public async Task<bool> MemberExistsAsync(int id)
    {
        return await _membreRepository.CheckIfMemberExistsAsync(id);
    }
}