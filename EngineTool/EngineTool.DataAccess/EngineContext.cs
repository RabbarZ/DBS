using EngineTool.DataAccess.Entities;
using EngineTool.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EngineTool.DataAccess;

internal sealed class EngineContext(DbContextOptions options) : DbContext(options), IEngineContext
{
    public DbSet<Game> Games { get; set; }

    public DbSet<Engine> Engines { get; set; }

    public DbSet<Rating> Ratings { get; set; }

    public DbSet<PlayerStats> PlayerStats { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>().HasAlternateKey(g => g.IgdbId);
        modelBuilder.Entity<PlayerStats>().HasAlternateKey(pc => new { pc.GameId, pc.Timestamp });
        modelBuilder.Entity<Rating>().HasAlternateKey(r => new { r.GameId, r.Timestamp });
        base.OnModelCreating(modelBuilder);
    }
}