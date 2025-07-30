using bibliotheque_back_end.Data;
using bibliotheque_back_end.Models.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace bibliotheque_back_end.Models.Service;

public class StatistiquesService : IStatistiquesService
{
    private readonly BibliothequeDb _context;

    public StatistiquesService(BibliothequeDb context)
    {
        _context = context;
    }

    public async Task<int> GetNombreEmpruntsEnCoursAsync()
    {
        return await _context.Emprunts
            .CountAsync(e => e.DateRetour == null);
    }

    public async Task<int> GetNombreEmpruntsEnRetardAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var maxLoanDays = 14;

        return await _context.Emprunts
            .CountAsync(e => e.DateRetour == null &&
                            e.DateEmprunt.AddDays(maxLoanDays) < today);
    }

    public async Task<List<object>> GetTop5AuteursPopulairesAsync()
    {
        var result = await _context.Livres
            .Join(_context.EmpruntLivres,
                  livre => livre.Id,
                  el => el.IdLivre,
                  (livre, el) => livre.Auteur)
            .Where(auteur => !string.IsNullOrEmpty(auteur))
            .GroupBy(auteur => auteur)
            .Select(g => new {
                Auteur = g.Key,
                NombreEmprunts = g.Count()
            })
            .OrderByDescending(x => x.NombreEmprunts)
            .Take(5)
            .ToListAsync();

        return result.Cast<object>().ToList();
    }


}
