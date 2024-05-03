using EngineTool.Entities;
using Microsoft.EntityFrameworkCore;

namespace EngineTool.Interfaces
{
    public interface IEngineContext
    {
        DbSet<Game> Games { get; set; }

        DbSet<Engine> Engines { get; set; }

        DbSet<Rating> Ratings { get; set; }

        DbSet<PlayerStats> PlayerStats { get; set; }
    }
}
