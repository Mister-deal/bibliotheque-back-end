namespace bibliotheque_back_end.Models.Service.Interface;

public interface IEmployeService
{
    IEnumerable<Employe> GetAllEmployees();
    Employe GetEmployeeById(int id);
    Employe GetEmployeeByEmail(string email);
    
    Employe AddEmployee(Employe newEmployee, string dataPassword);
    
    Employe UpdateEmployee(int id, Employe updatedEmployee);
    Employe UpdateEmployeeRole(int id, Role updatedRole);
    Employe UpdateEmployeePassword(int id, string oldPassword, string newPassword);

    Employe DeleteEmployee(int id);
    
    bool EmployeeExists(int id);
    
}