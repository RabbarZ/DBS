using EngineTool;
using EngineToolViewer.Models;
using Microsoft.EntityFrameworkCore;

namespace EngineToolViewer.Services
{
    public static class ViewService
    {
        public static List<EngineView> GetEngineViews()
        {
            using (EngineContext context = new())
            {
                List<EngineView> views = [];

                foreach (var e in context.Engines.AsNoTracking().Where(e => e.Games.Any()).Include(e => e.Games).ThenInclude(g => g.Ratings).Include(e => e.Games).ThenInclude(g => g.PlayerStats))
                {
                    views.Add(new EngineView
                    {
                        Name = e.Name,
                        GamesCount = e.Games.Count,
                        GameNames = e.Games.Select(g => g.Name).Take(100).Aggregate((acc, curr) => acc + ", " + curr),
                        AverageRating = e.Games.Average(g => g.Ratings.Average(r => r.Score)),
                        AveragePlayerCount = e.Games.Average(g => g.PlayerStats.Average(ps => ps.PlayerCount))
                    });
                }

                return [.. views.OrderByDescending(e => e.AveragePlayerCount)];
            }
        }

        public static List<GameView> GetGameViews(int count)
        {
            using (EngineContext context = new())
            {
                List<GameView> views = [];

                foreach (var game in context.Games.AsNoTracking().Include(g => g.Engines).Include(g => g.PlayerStats).Include(g => g.Ratings).OrderByDescending(g => g.PlayerStats.Average(ps => ps.PlayerCount)).Take(count))
                {
                    views.Add(new GameView
                    {
                        Name = game.Name,
                        EnginesCount = game.Engines.Count,
                        EngineNames = game.Engines.Select(e => e.Name).Aggregate((acc, curr) => acc + ", " + curr),
                        AverageRating = game.Ratings.Average(r => r.Score),
                        AveragePlayerCount = game.PlayerStats.Average(ps => ps.PlayerCount)
                    });
                }

                return views;
            }
        }
    }
}
