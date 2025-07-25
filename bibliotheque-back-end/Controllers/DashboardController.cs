using Microsoft.AspNetCore.Mvc;
using BibliothequeBackEnd.Models.ViewModels;

namespace BibliothequeBackEnd.Controllers
{
    public class DashboardController : Controller
    {
        // Ici pour injecter tes services/repository plus tard (Si on en fait)

        public IActionResult Index()
        {
            // Créer et remplir le ViewModel
            var viewModel = new DashboardViewModel
            {
                TotalBooks = GetTotalBooks(),
                TotalUsers = GetTotalUsers(),
                TodayLoans = GetTodayLoans(),
                PendingReturns = GetPendingReturns(),
                RecentActivities = GetRecentActivities(),
                PopularBooks = GetPopularBooks()
            };

            return View(viewModel);
        }

        // Méthodes temporaires (à remplacer par les services plus tard)
        private int GetTotalBooks() => 156; // Exemple
        private int GetTotalUsers() => 89;
        private int GetTodayLoans() => 12;
        private int GetPendingReturns() => 7;

        private List<RecentActivityViewModel> GetRecentActivities()
        {
            return new List<RecentActivityViewModel>
            {
                new RecentActivityViewModel
                {
                    Title = "Nouvel emprunt",
                    Description = "Jean Dupont a emprunté 'Le Petit Prince'",
                    Time = DateTime.Now.AddMinutes(-15),
                    Icon = "book",
                    BadgeColor = "success"
                },
                new RecentActivityViewModel
                {
                    Title = "Retour effectué",
                    Description = "Marie Martin a rendu '1984'",
                    Time = DateTime.Now.AddHours(-2),
                    Icon = "undo",
                    BadgeColor = "info"
                }
            };
        }

        private List<PopularBookViewModel> GetPopularBooks()
        {
            return new List<PopularBookViewModel>
            {
                new PopularBookViewModel { Id = 1, Title = "Le Petit Prince", Author = "Saint-Exupéry", LoanCount = 45 },
                new PopularBookViewModel { Id = 2, Title = "1984", Author = "Orwell", LoanCount = 38 }
            };
        }
    }
}
