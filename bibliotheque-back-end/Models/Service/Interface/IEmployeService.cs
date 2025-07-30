namespace bibliotheque_back_end.Models.Service.Interface;

public interface IEmployeService
{
    Task<IEnumerable<Employe>> GetAllEmployeesAsync();
    Task<Employe?> GetEmployeeByIdAsync(int id);
    Task<Employe?> GetEmployeeByEmailAsync(string email);
    
    Task<Employe> AddEmployeeAsync(Employe newEmployee, string dataPassword);
    
    Task<Employe?> UpdateEmployeeAsync(int id, Employe updatedEmployee);
    Task<Employe?> DeleteEmployeeAsync(int id);
    
    Task<bool> EmployeeExistsAsync(int id);
}