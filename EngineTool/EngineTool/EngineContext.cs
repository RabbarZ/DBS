using EngineTool.Models;
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
            optionsBuilder.UseMySQL("Server=db-vm-38.el.eee.intern:3306;Database=EngineTool;User ID=HSLUUser;Password=DasTeam;");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
