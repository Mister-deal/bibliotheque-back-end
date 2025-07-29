using bibliotheque_back_end.Models.repositery;

namespace bibliotheque_back_end.Models.Service.Interface;

public class EmployeService: IEmployeService
{
    private readonly IEmployeRepository _repository;

    public EmployeService(IEmployeRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<Employe> GetAllEmployees()
    {
        return _repository.GetAllEmployees();
    }

    public Employe GetEmployeeById(int id)
    {
        if(id <= 0) throw new ArgumentException(nameof(id));
        var employee = _repository.GetEmployee(id);
        if(id == null) throw new ArgumentException(nameof(id));
        return employee;
    }

    public Employe GetEmployeeByEmail(string email)
    {
        return _repository.GetEmployeeByEmail(email);
    }

    public Employe AddEmployee(Employe newEmployee, string dataPassword)
    {
        if(newEmployee != null) throw new ArgumentException(nameof(newEmployee));
        if (string.IsNullOrWhiteSpace(dataPassword))
        {
            throw new ArgumentException("le mot de passe ne peut être nul ou vide", nameof(dataPassword));
        }
        if (string.IsNullOrWhiteSpace(newEmployee.Email) ||
            string.IsNullOrWhiteSpace(newEmployee.Nom) ||
            string.IsNullOrWhiteSpace(newEmployee.Prenom))
        {
            throw new ArgumentException("Email, nom, et prénom sont nécessaire pour la création d'un nouveau membre.", nameof(newEmployee));
        }

        if (newEmployee.Role == null)
        {
            throw new ArgumentNullException("le role ne peut être nul", nameof(newEmployee));
        }
        if (newEmployee.Role == default(Role))
        {
            newEmployee.Role = Role.Bibliothecaire;
        }
        
        if (_repository.GetAllEmployees().Any(m => m.Email.Equals(newEmployee.Email, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException($"un membre avec le même mail '{newEmployee.Email}' existe déjà.");
        }
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(dataPassword);
        newEmployee.MotDePasse = passwordHash;
        _repository.AddEmployee(newEmployee);
        return newEmployee;
    }

    public Employe UpdateEmployee(int id, Employe updatedEmployee)
    {
        if(updatedEmployee == null) throw new ArgumentException(nameof(updatedEmployee));
        if (id <= 0) throw new ArgumentException(nameof(id));
        if(id != updatedEmployee.Id) throw new ArgumentException(nameof(updatedEmployee));
        var existingMember = _repository.GetEmployee(id);
        if(existingMember == null) throw new ArgumentException(nameof(id));
        
        existingMember.MotDePasse = updatedEmployee.MotDePasse;
        existingMember.Nom = updatedEmployee.Nom;
        existingMember.Email = updatedEmployee.Email;
        existingMember.Prenom = updatedEmployee.Prenom;
        _repository.UpdateEmployee(existingMember);
        return updatedEmployee;
    }

    public Employe UpdateEmployeeRole(int id, Role updatedRole)
    {
        if(id <= 0) throw new ArgumentException(nameof(id));
        var existingEmployee = _repository.GetEmployee(id);
        if(existingEmployee == null) throw new ArgumentException(nameof(id));
        existingEmployee.Role = updatedRole;
        _repository.UpdateEmployee(existingEmployee);
        return existingEmployee;
    }

    public Employe UpdateEmployeePassword(int id, string oldPassword, string newPassword)
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
        
        var employee = _repository.GetEmployee(id);
        if (employee == null) throw new ArgumentException(nameof(id));
        if (!BCrypt.Net.BCrypt.Verify(oldPassword, employee.MotDePasse))
        {
            throw new UnauthorizedAccessException("mot de passe incorrect !");
        }
        employee.MotDePasse = BCrypt.Net.BCrypt.HashPassword(newPassword);
        _repository.UpdateEmployee(employee);
        return employee;
    }

    public Employe DeleteEmployee(int id)
    {
        if(id <= 0) throw new ArgumentException(nameof(id));
        var existingEmployee = _repository.GetEmployee(id);
        if(existingEmployee == null) throw new ArgumentException(nameof(id));

        var deletedEmployee = existingEmployee;
        _repository.DeleteEmployee(deletedEmployee);
        return deletedEmployee;
    }

    public bool EmployeeExists(int id)
    {
        return _repository.CheckIfEmployeeExists(id);
    }
}