using bibliotheque_back_end.Data;
using bibliotheque_back_end.Models.repositery;

namespace bibliotheque_back_end.Models.Service.Interface;

public class EmployeService : IEmployeService
{
    private readonly IEmployeRepository _repository;
    private readonly BibliothequeDb _context; // Ajout du DbContext pour SaveChangesAsync()

    // Le constructeur doit maintenant prendre BibliothequeDb pour SaveChangesAsync()
    public EmployeService(IEmployeRepository repository, BibliothequeDb context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task<IEnumerable<Employe>> GetAllEmployeesAsync()
    {
        return await _repository.GetAllEmployeesAsync();
    }

    public async Task<Employe?> GetEmployeeByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("L'ID de l'employé doit être supérieur à zéro.", nameof(id));
        }

        var employee = await _repository.GetEmployeeAsync(id);
        // Pas besoin de jeter une ArgumentException si l'employé est null, un retour null est attendu par l'interface.
        // C'est à la couche appelante (contrôleur) de décider quoi faire avec un retour null (ex: NotFound).
        return employee;
    }

    public async Task<Employe?> GetEmployeeByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("L'email ne peut pas être nul ou vide.", nameof(email));
        }
        return await _repository.GetEmployeeByEmailAsync(email);
    }

    public async Task<Employe> AddEmployeeAsync(Employe newEmployee, string dataPassword)
    {
        // Correction de la logique de vérification de null :
        // Si newEmployee est null, vous devriez lancer une exception, pas si ce n'est PAS null.
        if (newEmployee == null)
        {
            throw new ArgumentNullException(nameof(newEmployee), "L'objet employé ne peut pas être nul.");
        }
        if (string.IsNullOrWhiteSpace(dataPassword))
        {
            throw new ArgumentException("Le mot de passe ne peut être nul ou vide.", nameof(dataPassword));
        }
        if (string.IsNullOrWhiteSpace(newEmployee.Email) ||
            string.IsNullOrWhiteSpace(newEmployee.Nom) ||
            string.IsNullOrWhiteSpace(newEmployee.Prenom))
        {
            throw new ArgumentException("Email, nom et prénom sont nécessaires pour la création d'un nouvel employé.", nameof(newEmployee));
        }

        if (newEmployee.Role == null) // La propriété Role est-elle nullable? Si oui, OK. Si non, ce check est superflu pour un type enum.
        {
            throw new ArgumentNullException("Le rôle ne peut être nul.", nameof(newEmployee));
        }
        // Pour les enums, 'default(Role)' est souvent le premier élément (0).
        // Il est plus robuste de vérifier si le rôle est un membre défini de l'enum si ce n'est pas le cas.
        if (newEmployee.Role == default(Role)) // Ou une autre logique pour un rôle par défaut
        {
            newEmployee.Role = Role.Bibliothecaire;
        }
        
        // Utilisation correcte de la méthode asynchrone du repository pour vérifier l'existence par email
        if (await _repository.ExistsByEmailAsync(newEmployee.Email))
        {
            throw new InvalidOperationException($"Un employé avec le même email '{newEmployee.Email}' existe déjà.");
        }
        
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(dataPassword);
        newEmployee.MotDePasse = passwordHash;
        
        await _repository.AddEmployeeAsync(newEmployee);
        await _context.SaveChangesAsync(); // Sauvegarde asynchrone des changements

        return newEmployee;
    }

    public async Task<Employe?> UpdateEmployeeAsync(int id, Employe updatedEmployee)
    {
        if (updatedEmployee == null)
        {
            throw new ArgumentNullException(nameof(updatedEmployee), "L'objet employé mis à jour ne peut pas être nul.");
        }
        if (id <= 0)
        {
            throw new ArgumentException("L'ID de l'employé doit être supérieur à zéro.", nameof(id));
        }
        // Vérifiez la correspondance des IDs ici si nécessaire. L'interface ne l'exige pas explicitement,
        // mais c'est une bonne pratique si l'ID de l'URL est la source de vérité.
        if (id != updatedEmployee.Id)
        {
             throw new ArgumentException("L'ID de l'URL ne correspond pas à l'ID de l'employé fourni.", nameof(updatedEmployee));
        }

        var existingEmployee = await _repository.GetEmployeeAsync(id);
        if (existingEmployee == null)
        {
            // Retourne null si l'employé n'existe pas, cohérent avec l'interface.
            return null;
        }
        
        // Mettre à jour les propriétés de l'entité existante
        // Attention: ne pas copier directement le MotDePasse si vous ne le mettez pas à jour ici.
        // updatedEmployee.MotDePasse; // Ne pas toucher au mot de passe ici à moins de le gérer explicitement
        existingEmployee.Nom = updatedEmployee.Nom;
        existingEmployee.Email = updatedEmployee.Email;
        existingEmployee.Prenom = updatedEmployee.Prenom;
        // Gérer le rôle si nécessaire, ex: existingEmployee.Role = updatedEmployee.Role;

        await _repository.UpdateEmployeeAsync(existingEmployee);
        await _context.SaveChangesAsync(); // Sauvegarde asynchrone des changements

        return existingEmployee; // Retourne l'entité mise à jour depuis la base de données
    }

    public async Task<Employe?> UpdateEmployeeRoleAsync(int id, Role updatedRole)
    {
        if (id <= 0)
        {
            throw new ArgumentException("L'ID de l'employé doit être supérieur à zéro.", nameof(id));
        }
        // Vérifier si le rôle est une valeur d'enum valide si nécessaire
        // if (!Enum.IsDefined(typeof(Role), updatedRole)) { throw new ArgumentOutOfRangeException(nameof(updatedRole), "Rôle non valide."); }

        var existingEmployee = await _repository.GetEmployeeAsync(id);
        if (existingEmployee == null)
        {
            return null; // Retourne null si l'employé n'existe pas.
        }
        
        existingEmployee.Role = updatedRole; // Mise à jour directe du rôle
        
        await _repository.UpdateEmployeeAsync(existingEmployee);
        await _context.SaveChangesAsync(); // Sauvegarde asynchrone des changements

        return existingEmployee;
    }

    public async Task<Employe?> UpdateEmployeePasswordAsync(int id, string oldPassword, string newPassword)
    {
        if (id <= 0)
        {
            throw new ArgumentException("L'ID de l'employé doit être supérieur à zéro.", nameof(id));
        }
        if (string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword))
        {
            throw new ArgumentException("Les mots de passe ne peuvent être vides.", nameof(oldPassword)); // ou nameof(newPassword)
        }

        if (newPassword == oldPassword)
        {
            throw new InvalidOperationException("Le nouveau mot de passe ne peut être similaire à l'ancien mot de passe.");
        }
        
        var employee = await _repository.GetEmployeeAsync(id);
        if (employee == null)
        {
            return null; // Retourne null si l'employé n'existe pas.
        }
        
        // Vérification du mot de passe existant
        if (!BCrypt.Net.BCrypt.Verify(oldPassword, employee.MotDePasse))
        {
            throw new UnauthorizedAccessException("Mot de passe incorrect !");
        }
        
        employee.MotDePasse = BCrypt.Net.BCrypt.HashPassword(newPassword);
        
        await _repository.UpdateEmployeeAsync(employee);
        await _context.SaveChangesAsync(); // Sauvegarde asynchrone des changements

        return employee;
    }

    public async Task<Employe?> DeleteEmployeeAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("L'ID de l'employé doit être supérieur à zéro.", nameof(id));
        }

        var existingEmployee = await _repository.GetEmployeeAsync(id);
        if (existingEmployee == null)
        {
            return null; // Retourne null si l'employé n'existe pas.
        }

        await _repository.DeleteEmployeeAsync(existingEmployee);
        await _context.SaveChangesAsync(); // Sauvegarde asynchrone des changements

        return existingEmployee; // Retourne l'employé qui a été supprimé
    }

    public async Task<bool> EmployeeExistsAsync(int id)
    {
        return await _repository.CheckIfEmployeeExistsAsync(id);
    }
}