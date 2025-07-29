namespace bibliotheque_back_end.Models.Service.Interface;

public interface IMembreService
{
    Task<IEnumerable<Membre>> GetAllMembersAsync();
    Task<Membre?> GetMemberByIdAsync(int id); // Peut retourner null si non trouvé
    Task<Membre?> GetMemberByEmailAsync(string email); // Peut retourner null si non trouvé
    
    Task<Membre> AddMemberAsync(Membre newMember, string dataPassword);
    
    Task<Membre?> UpdateMemberAsync(int id, Membre updatedMember); // Peut retourner null si non trouvé
    Task<Membre?> UpdatePasswordMemberAsync(int id, string oldPassword, string newPassword); // Peut retourner null si non trouvé ou si les mots de passe ne correspondent pas

    Task<Membre?> DeleteMemberAsync(int id); // Peut retourner null si non trouvé
    
    Task<bool> MemberExistsAsync(int id);
}