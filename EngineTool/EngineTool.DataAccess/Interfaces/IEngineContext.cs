using EngineTool.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace EngineTool.DataAccess.Interfaces;

public interface IEngineContext
{
    DbSet<Game> Games { get; set; }

    DbSet<Engine> Engines { get; set; }

    DbSet<Rating> Ratings { get; set; }

    DbSet<PlayerStats> PlayerStats { get; set; }

    DbSet<T> Set<T>() where T : class;

    int SaveChanges();
}