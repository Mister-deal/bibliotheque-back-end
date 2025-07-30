using Microsoft.AspNetCore.Mvc;
using BibliothequeBackEnd.Models.ViewModels;

namespace bibliotheque_back_end.Controllers
{
    public class DashboardController : Controller
    {
        // Ici pour injecter tes services/repository plus tard (Si on en fait)
        // private readonly ILivreService _livreService;
        // private readonly IUserService _userService;
        // private readonly IEmpruntService _empruntService;

        public async Task<IActionResult> Index()
        {
            try
            {
                // Créer et remplir le ViewModel
                var viewModel = new DashboardViewModel
                {
                    TotalBooks = await GetTotalBooksAsync(),
                    TotalUsers = await GetTotalUsersAsync(),
                    TodayLoans = await GetTodayLoansAsync(),
                    PendingReturns = await GetPendingReturnsAsync(),
                    RecentActivities = await GetRecentActivitiesAsync(),
                    PopularBooks = await GetPopularBooksAsync()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Log l'erreur si vous avez un système de logging
                ViewBag.ErrorMessage = "Erreur lors du chargement du dashboard";

                // Retourner un ViewModel vide en cas d'erreur
                return View(new DashboardViewModel
                {
                    RecentActivities = new List<RecentActivityViewModel>(),
                    PopularBooks = new List<PopularBookViewModel>()
                });
            }
        }

        // Méthodes temporaires (à remplacer par les services plus tard)
        private async Task<int> GetTotalBooksAsync()
        {
            // Simulation d'une opération async
            await Task.Delay(1);
            return 156; // Plus tard: return await _livreService.GetAvailableBooksCountAsync();
        }

        private async Task<int> GetTotalUsersAsync()
        {
            await Task.Delay(1);
            return 89; // Plus tard: return await _userService.GetActiveUsersCountAsync();
        }

        private async Task<int> GetTodayLoansAsync()
        {
            await Task.Delay(1);
            return 12; // Plus tard: return await _empruntService.GetTodayLoansCountAsync();
        }

        private async Task<int> GetPendingReturnsAsync()
        {
            await Task.Delay(1);
            return 7; // Plus tard: return await _empruntService.GetOverdueReturnsCountAsync();
        }

        private async Task<List<RecentActivityViewModel>> GetRecentActivitiesAsync()
        {
            await Task.Delay(1);

            // Données d'exemple plus variées
            return new List<RecentActivityViewModel>
            {
                new RecentActivityViewModel
                {
                    Title = "Nouvel emprunt",
                    Description = "Jean Dupont a emprunté 'Le Petit Prince'",
                    Time = DateTime.Now.AddMinutes(-15),
                    Icon = "handshake", // Changé pour être cohérent avec AdminLTE
                    BadgeColor = "success"
                },
                new RecentActivityViewModel
                {
                    Title = "Retour effectué",
                    Description = "Marie Martin a rendu '1984'",
                    Time = DateTime.Now.AddHours(-2),
                    Icon = "undo",
                    BadgeColor = "info"
                },
                new RecentActivityViewModel
                {
                    Title = "Nouveau livre ajouté",
                    Description = "Ajout de 'Les Misérables' à la bibliothèque",
                    Time = DateTime.Now.AddHours(-4),
                    Icon = "plus",
                    BadgeColor = "primary"
                },
                new RecentActivityViewModel
                {
                    Title = "Rappel de retard",
                    Description = "Email envoyé à Pierre Durand pour retard",
                    Time = DateTime.Now.AddHours(-6),
                    Icon = "exclamation-triangle",
                    BadgeColor = "warning"
                },
                new RecentActivityViewModel
                {
                    Title = "Nouvel utilisateur",
                    Description = "Sophie Leblanc s'est inscrite",
                    Time = DateTime.Now.AddDays(-1),
                    Icon = "user-plus",
                    BadgeColor = "success"
                }
            };
        }

        private async Task<List<PopularBookViewModel>> GetPopularBooksAsync()
        {
            await Task.Delay(1);

            // Plus de livres populaires pour tester l'affichage
            return new List<PopularBookViewModel>
            {
                new PopularBookViewModel
                {
                    Id = 1,
                    Title = "Le Petit Prince",
                    Author = "Antoine de Saint-Exupéry",
                    LoanCount = 45
                },
                new PopularBookViewModel
                {
                    Id = 2,
                    Title = "1984",
                    Author = "George Orwell",
                    LoanCount = 38
                },
                new PopularBookViewModel
                {
                    Id = 3,
                    Title = "Les Misérables",
                    Author = "Victor Hugo",
                    LoanCount = 32
                },
                new PopularBookViewModel
                {
                    Id = 4,
                    Title = "L'Étranger",
                    Author = "Albert Camus",
                    LoanCount = 28
                },
                new PopularBookViewModel
                {
                    Id = 5,
                    Title = "Le Comte de Monte-Cristo",
                    Author = "Alexandre Dumas",
                    LoanCount = 25
                }
            };
        }

        // Méthode utilitaire pour simuler une variation des statistiques
        private int GetRandomizedStat(int baseValue, int variation = 5)
        {
            var random = new Random();
            return baseValue + random.Next(-variation, variation + 1);
        }
    }
}