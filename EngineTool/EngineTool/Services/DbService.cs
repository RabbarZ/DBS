using EngineTool.Entities;
using EngineTool.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EngineTool.Services
{
    public class DbService(EngineContext context) : IDbService, IDisposable
    {
        private readonly EngineContext context = context;

        public void EnsureDbExists()
        {
            this.context.Database.EnsureCreated();
        }

        public Game GetGameByIdgbId(int igdbId)
        {
            return this.context.Games.AsNoTracking().SingleOrDefault(g => g.IgdbId == igdbId);
        }

        public Engine GetEngineByIdgbId(int igdbId)
        {
            return this.context.Engines.AsNoTracking().SingleOrDefault(e => e.IgdbId == igdbId);
        }

        public bool GetEngineContainsGame(Guid engineId, Guid gameId)
        {
            return this.context.Engines.AsNoTracking().Where(e => e.Id == engineId).Any(e => e.Games.Any(g => g.Id == gameId));
        }

        public Engine AddEngine(Engine engine)
        {
            this.context.Engines.Add(engine);
            this.context.SaveChanges();

            return engine;
        }

        public Engine UpdateEngine(Engine engine)
        {
            this.context.Engines.Update(engine);
            this.context.SaveChanges();

            return engine;
        }

        public Game AddGame(Game game)
        {
            this.context.Games.Add(game);
            this.context.SaveChanges();

            return game;
        }

        public PlayerStats AddPlayerStats(PlayerStats playerStats)
        {
            this.context.PlayerStats.Add(playerStats);
            this.context.SaveChanges();

            return playerStats;
        }

        public Rating AddRating(Rating rating)
        {
            this.context.Ratings.Add(rating);
            this.context.SaveChanges();

            return rating;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.context.Dispose();
            }
        }
    }
}
