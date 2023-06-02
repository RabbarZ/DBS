using EngineTool.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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
            optionsBuilder.UseMySQL("Server=db-vm-38.el.eee.intern;Database=EngineTool;User ID=HSLUUser;Password=DasTeam;");
            optionsBuilder.LogTo(x => Debug.WriteLine(x));
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
