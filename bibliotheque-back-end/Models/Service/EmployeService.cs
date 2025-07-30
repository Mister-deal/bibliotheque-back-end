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
        if (id != updatedEmployee.Id)
        {
            throw new ArgumentException("L'ID de l'URL ne correspond pas à l'ID de l'employé fourni.", nameof(updatedEmployee));
        }

        var existingEmployee = await _repository.GetEmployeeAsync(id);
        if (existingEmployee == null)
        {
            return null;
        }
        
        existingEmployee.Nom = updatedEmployee.Nom;
        existingEmployee.Prenom = updatedEmployee.Prenom;
        existingEmployee.Email = updatedEmployee.Email;
        
        if (existingEmployee.Role != updatedEmployee.Role)
        {
            existingEmployee.Role = updatedEmployee.Role;
        }
        
        
        if (!string.IsNullOrWhiteSpace(updatedEmployee.MotDePasse) &&
            updatedEmployee.MotDePasse != existingEmployee.MotDePasse) // Compare against existing for changes, but ensure it's hashed later
        {
            // Validate the new password format if necessary (e.g., using a regular expression)
            // This validation typically happens at the DTO or model level using data annotations.

            // Hash the new password before storing it
            existingEmployee.MotDePasse = BCrypt.Net.BCrypt.HashPassword(updatedEmployee.MotDePasse);
        }

        await _repository.UpdateEmployeeAsync(existingEmployee);
        await _context.SaveChangesAsync();

        return existingEmployee;
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