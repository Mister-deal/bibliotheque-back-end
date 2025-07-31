namespace bibliotheque_back_end.Models.Service.Interface;

public interface IAuthService
{
    /// <summary>
    /// Tente d'authentifier un utilisateur (Employé ou Membre) et génère un token JWT.
    /// </summary>
    /// <param name="email">L'email de l'utilisateur.</param>
    /// <param name="password">Le mot de passe en clair de l'utilisateur.</param>
    /// <returns>Le token JWT si l'authentification est réussie, sinon null.</returns>
    Task<string?> AuthenticateUser(string email, string password);
    
    Task<bool> RegisterUser(string nom, string prenom, string email, string password);

}