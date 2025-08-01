using bibliotheque_back_end.Models;
namespace bibliotheque_back_end.Models.repositery;
public interface IEmployeRepository
{
    //methode async pour récupérer tous employés
    Task<IEnumerable<Employe>> GetAllEmployeesAsync();
    //methode async pour récupérer employee par son ID
    Task<Employe?> GetEmployeeAsync(int id);
    //Methode async recupération employee par mail
    Task<Employe?> GetEmployeeByEmailAsync(string email);
    //création employee
    Task AddEmployeeAsync(Employe emp);
    //modif employee
    Task UpdateEmployeeAsync(Employe emp);
    //suppression employee
    Task DeleteEmployeeAsync(Employe emp);
    //methode bool afin de voir si un employee existe
    Task<bool> CheckIfEmployeeExistsAsync(int id);
    //même methode mais avec l'email
    Task<bool> ExistsByEmailAsync(string email);
}