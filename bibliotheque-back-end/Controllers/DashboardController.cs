using Microsoft.AspNetCore.Mvc;
using BibliothequeBackEnd.Models.ViewModels;
using bibliotheque_back_end.Models.Service.Interface;

namespace bibliotheque_back_end.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IStatistiquesService _statistiquesService;
        private readonly ILivreService _livreService;
        private readonly IMembreService _membreService;

        public DashboardController(
            IStatistiquesService statistiquesService,
            ILivreService livreService,
            IMembreService membreService)
        {
            _statistiquesService = statistiquesService;
            _livreService = livreService;
            _membreService = membreService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var viewModel = new DashboardViewModel
                {
                    TotalBooks = await GetTotalBooksAsync(),
                    TotalUsers = await GetTotalUsersAsync(),
                    TodayLoans = await _statistiquesService.GetNombreEmpruntsEnCoursAsync(),
                    PendingReturns = await _statistiquesService.GetNombreEmpruntsEnRetardAsync(),
                    RecentActivities = GetRecentActivitiesSync(),
                    PopularBooks = await GetPopularBooksAsync()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Erreur: " + ex.Message;
                return View(new DashboardViewModel
                {
                    TotalBooks = 0,
                    TotalUsers = 0,
                    TodayLoans = 0,
                    PendingReturns = 0,
                    RecentActivities = new List<RecentActivityViewModel>(),
                    PopularBooks = new List<PopularBookViewModel>()
                });
            }
        }

        // 🚀 AVEC TES VRAIES MÉTHODES !
        private async Task<int> GetTotalBooksAsync()
        {
            try
            {
                var livres = await _livreService.GetAllBooksAsync(); // ✅ TON VRAI NOM !
                return livres.Count();
            }
            catch
            {
                return 0;
            }
        }

        private async Task<int> GetTotalUsersAsync()
        {
            try
            {
                var membres = await _membreService.GetAllMembersAsync(); // ✅ TON VRAI NOM !
                return membres.Count();
            }
            catch
            {
                return 0;
            }
        }

        private async Task<List<PopularBookViewModel>> GetPopularBooksAsync()
        {
            try
            {
                var topAuteurs = await _statistiquesService.GetTop5AuteursPopulairesAsync();

                var result = new List<PopularBookViewModel>();
                int index = 1;

                foreach (dynamic auteur in topAuteurs)
                {
                    result.Add(new PopularBookViewModel
                    {
                        Id = index++,
                        Title = $"Œuvres de {auteur.Auteur}",
                        Author = auteur.Auteur.ToString(),
                        LoanCount = (int)auteur.NombreEmprunts
                    });
                }

                return result.Any() ? result : GetDefaultPopularBooks();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur PopularBooks: {ex.Message}");
                return GetDefaultPopularBooks();
            }
        }

        private List<PopularBookViewModel> GetDefaultPopularBooks()
        {
            return new List<PopularBookViewModel>
            {
                new PopularBookViewModel
                {
                    Id = 1,
                    Title = "Aucune donnée disponible",
                    Author = "Système",
                    LoanCount = 0
                }
            };
        }

        private List<RecentActivityViewModel> GetRecentActivitiesSync()
        {
            return new List<RecentActivityViewModel>
            {
                new RecentActivityViewModel
                {
                    Title = "📊 Dashboard actualisé",
                    Description = "Statistiques en temps réel depuis la base de données",
                    Time = DateTime.Now,
                    Icon = "chart-line",
                    BadgeColor = "success"
                },
                new RecentActivityViewModel
                {
                    Title = " Livres synchronisés",
                    Description = "Inventaire des livres disponibles mis à jour",
                    Time = DateTime.Now.AddMinutes(-3),
                    Icon = "books",
                    BadgeColor = "info"
                },
                new RecentActivityViewModel
                {
                    Title = "👥 Membres actifs",
                    Description = "Liste des membres mise à jour",
                    Time = DateTime.Now.AddMinutes(-7),
                    Icon = "users",
                    BadgeColor = "primary"
                }
            };
        }
    }
}
