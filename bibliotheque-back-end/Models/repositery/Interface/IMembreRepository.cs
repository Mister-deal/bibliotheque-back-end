namespace bibliotheque_back_end.Models.repositery;

public interface IMembreRepository
{
    Task<IEnumerable<Membre>> GetAllMembersAsync();
    Task<Membre?> GetMemberAsync(int id);
    Task<Membre?> GetMemberByEmailAsync(string email);
    Task AddMemberAsync(Membre member);
    Task UpdateMemberAsync(Membre member);
    Task DeleteMemberAsync(Membre member);
    Task<bool> CheckIfMemberExistsAsync(int id);
}