namespace bibliotheque_back_end.Models.Service.Interface;

public interface IStatistiquesService
{
    // Partie Dashboard statistiques
    Task<int> GetNombreEmpruntsEnCoursAsync();
    Task<int> GetNombreEmpruntsEnRetardAsync();
    Task<List<object>> GetTop5AuteursPopulairesAsync();
}
