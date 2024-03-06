using EngineTool.Entities;
using Microsoft.EntityFrameworkCore;

namespace EngineTool.Services
{
    public static class DbService
    {
        public static void EnsureDbExists()
        {
            using (var context = new EngineContext())
            {
                context.Database.EnsureCreated();
            }
        }

        public static Game GetGameByIdgbId(int igdbId)
        {
            using (var context = new EngineContext())
            {
                return context.Games.AsNoTracking().SingleOrDefault(g => g.IgdbId == igdbId);
            }
        }

        public static Engine GetEngineByIdgbId(int igdbId)
        {
            using (var context = new EngineContext())
            {
                return context.Engines.AsNoTracking().SingleOrDefault(e => e.IgdbId == igdbId);
            }
        }

        public static bool GetEngineContainsGame(Guid engineId, Guid gameId)
        {
            using (var context = new EngineContext())
            {
                return context.Engines.AsNoTracking().Where(e => e.Id == engineId).Any(e => e.Games.Any(g => g.Id == gameId));
            }
        }

        public static Engine AddEngine(Engine engine)
        {
            using (var context = new EngineContext())
            {
                context.Engines.Add(engine);
                context.SaveChanges();
            }

            return engine;
        }

        public static Engine UpdateEngine(Engine engine)
        {
            using (var context = new EngineContext())
            {
                context.Engines.Update(engine);
                context.SaveChanges();
            }

            return engine;
        }

        public static Game AddGame(Game game)
        {
            using (var context = new EngineContext())
            {
                context.Games.Add(game);
                context.SaveChanges();
            }

            return game;
        }

        public static PlayerStats AddPlayerStats(PlayerStats playerStats)
        {
            using (var context = new EngineContext())
            {
                context.PlayerStats.Add(playerStats);
                context.SaveChanges();
            }

            return playerStats;
        }

        public static Rating AddRating(Rating rating)
        {
            using (var context = new EngineContext())
            {
                context.Ratings.Add(rating);
                context.SaveChanges();
            }

            return rating;
        }
    }
}
