namespace BibliothequeBackEnd.Models.ViewModels
{
    public class DashboardViewModel
    {
        // Statistiques principales
        public int TotalBooks { get; set; }
        public int TotalUsers { get; set; }
        public int TodayLoans { get; set; }
        public int PendingReturns { get; set; }

        // Listes pour affichage
        public List<RecentActivityViewModel> RecentActivities { get; set; } = new List<RecentActivityViewModel>();
        public List<PopularBookViewModel> PopularBooks { get; set; } = new List<PopularBookViewModel>();
    }

    // ViewModels auxiliaires
    public class RecentActivityViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Time { get; set; }
        public string Icon { get; set; }
        public string BadgeColor { get; set; }
    }

    public class PopularBookViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int LoanCount { get; set; }
    }
}
