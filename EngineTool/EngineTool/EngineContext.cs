using EngineTool.Entities;
using Microsoft.EntityFrameworkCore;

namespace EngineTool
{
    public class EngineContext : DbContext
    {
        public DbSet<Game> Games { get; set; }

        public DbSet<Engine> Engines { get; set; }

        public DbSet<Rating> Ratings { get; set; }

        public DbSet<PlayerStats> PlayerStats { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=KILIAN\\SQLEXPRESS;Database=EngineTool;Integrated Security=SSPI;TrustServerCertificate=True;");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>().HasAlternateKey(g => g.IgdbId);
            modelBuilder.Entity<Game>().HasAlternateKey(g => g.SteamId);
            modelBuilder.Entity<Engine>().HasAlternateKey(e => e.IgdbId);
            modelBuilder.Entity<PlayerStats>().HasAlternateKey(pc => new { pc.GameId, pc.Timestamp });
            modelBuilder.Entity<Rating>().HasAlternateKey(r => new { r.GameId, r.Timestamp });
            base.OnModelCreating(modelBuilder);
        }
    }
}
