using bibliotheque_back_end.Models.repositery;
using bibliotheque_back_end.Models.Service.Interface;

namespace bibliotheque_back_end.Models.Service;

public class MembreService: IMembreService
{
    private readonly IMembreRepository _membreRepository;
    private readonly IEmpruntRepository _empruntRepository;

    public MembreService(IMembreRepository membreRepository, IEmpruntRepository empruntRepository)
    {
        _membreRepository = membreRepository;
        _empruntRepository = empruntRepository;
    }

    public IEnumerable<Membre> GetAllMembers()
    {
       return _membreRepository.GetAllMembers();
    }

    public Membre GetMemberById(int id)
    {
        if(id <= 0) throw new ArgumentException(nameof(id));
        var member = _membreRepository.GetMember(id);
        if(id == null) throw new ArgumentException(nameof(id));
        return member;
    }

    public Membre GetMemberByEmail(string email)
    {
        return _membreRepository.GetMemberByEmail(email);
    }

    public Membre AddMember(Membre newMember, string dataPassword)
    {
        if(newMember == null) throw new ArgumentException(nameof(newMember));

        if (string.IsNullOrWhiteSpace(dataPassword))
        {
            throw new ArgumentException("le mot de passe ne peut être nul ou vide", nameof(dataPassword));
        }
        if (string.IsNullOrWhiteSpace(newMember.Email) ||
            string.IsNullOrWhiteSpace(newMember.Nom) ||
            string.IsNullOrWhiteSpace(newMember.Prenom))
        {
            throw new ArgumentException("Email, nom, et prénom sont nécessaire pour la création d'un nouveau membre.", nameof(newMember));
        }
        
        if (_membreRepository.GetAllMembers().Any(m => m.Email.Equals(newMember.Email, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException($"un membre avec le même mail '{newMember.Email}' existe déjà.");
        }
       string passwordHash = BCrypt.Net.BCrypt.HashPassword(dataPassword);
       newMember.MotDePasse = passwordHash;
       _membreRepository.AddMember(newMember);
       return newMember;
    }

    public Membre UpdateMember(int id, Membre updatedMember)
    {
        if(updatedMember == null) throw new ArgumentException(nameof(updatedMember));
        if (id <= 0) throw new ArgumentException(nameof(id));
        if(id != updatedMember.Id) throw new ArgumentException(nameof(updatedMember));
        var existingMember = _membreRepository.GetMember(id);
        if(existingMember == null) throw new ArgumentException(nameof(id));
        
        existingMember.Nom = updatedMember.Nom;
        existingMember.Email = updatedMember.Email;
        existingMember.Prenom = updatedMember.Prenom;
        _membreRepository.UpdateMember(existingMember);
        return updatedMember;
    }

    public Membre UpdatePasswordMember(int id, string oldPassword, string newPassword)
    {
        if(id <= 0) throw new ArgumentException(nameof(id));
        if (string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword))
        {
            throw new ArgumentException("les mots de passe ne peuvent être vides");
        }

        if (newPassword == oldPassword)
        {
            throw new InvalidOperationException(
                "le nouveau mot de passe ne peut être similaire à l'ancien mot de passe");
        }
        
        var member = _membreRepository.GetMember(id);
        if (member == null) throw new ArgumentException(nameof(id));
        if (!BCrypt.Net.BCrypt.Verify(oldPassword, member.MotDePasse))
        {
            throw new UnauthorizedAccessException("mot de passe incorrect !");
        }
        member.MotDePasse = BCrypt.Net.BCrypt.HashPassword(newPassword);
        _membreRepository.UpdateMember(member);
        return member;
    }

    public Membre DeleteMember(int id)
    {
        if(id <= 0) throw new ArgumentException(nameof(id));
        var existingMember = _membreRepository.GetMember(id);
        if(existingMember == null) throw new ArgumentException(nameof(id));
        if (existingMember.emprunts.Any(e => e.DateRetour == null))
        {
            throw new InvalidOperationException($"Cannot delete member with ID {id} as they have active loans.");
        }

        var deletedMember = existingMember;
        _membreRepository.DeleteMember(deletedMember);
        return deletedMember;
    }

    public bool MemberExists(int id)
    {
        return _membreRepository.CheckIfMemberExists(id);
    }
}