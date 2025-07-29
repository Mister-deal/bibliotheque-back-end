namespace bibliotheque_back_end.Models.repositery;

public interface IEmployeRepository
{
    Task<IEnumerable<Employe>> GetAllEmployeesAsync();
    Task<Employe?> GetEmployeeAsync(int id);
    Task<Employe?> GetEmployeeByEmailAsync(string email);
    Task AddEmployeeAsync(Employe emp);
    Task UpdateEmployeeAsync(Employe emp);
    Task DeleteEmployeeAsync(Employe emp);
    Task<bool> CheckIfEmployeeExistsAsync(int id);
    Task<bool> ExistsByEmailAsync(string email);
}