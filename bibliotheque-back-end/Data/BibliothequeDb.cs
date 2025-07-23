using bibliotheque_back_end.Models;
using Microsoft.EntityFrameworkCore;

namespace bibliotheque_back_end.Data
{
    public class BibliothequeDb: DbContext
    {
        public BibliothequeDb(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Membre> Membres => Set<Membre>();
        public DbSet<Employe> Employes => Set<Employe>();
        public DbSet<Emprunt> Emprunts => Set<Emprunt>();
        public DbSet<EmpruntLivre> EmpruntLivres => Set<EmpruntLivre>();
        public DbSet<Reservation> Reservations => Set<Reservation>();
        public DbSet<Livre> Livres => Set<Livre>();
        
        
    }
}
