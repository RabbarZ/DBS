using EngineTool;
using EngineTool.Entities;
using EngineTool.Models;
using EngineTool.Services;

var igdbService = new IgdbService();
var steamService = new SteamService();
var timestamp = DateTime.UtcNow;

List<IgdbGame> games = await igdbService.GetGamesAsync(8500);
List<Game> dbGames = new();

int i = 1;
foreach (var game in games)
{
    string steamUrl = game.Websites.Single(w => w.Category == 13).Url;
    int steamId = 0;
    try
    {
        if (!int.TryParse(steamUrl.Split('/')[4], out steamId))
        {
            throw new Exception("Error while parsing steam app id.");
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
        continue;
    }

    Console.WriteLine($"{i} : {game.Name} : {steamUrl}");

    var playerCount = 0;
    IgdbRating rating = null;
    try
    {
        playerCount = await steamService.GetCurrentPlayerCountAsync(steamId);
        rating = await steamService.GetRatingAsync(steamId);

        if (rating == null)
        {
            throw new Exception("Failure due to unknown error.");
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
        continue;
    }

    var dbGame = new Game
    {
        Id = Guid.NewGuid(),
        Name = game.Name,
        IgdbId = game.Id,
        SteamId = steamId,
        PlayerStats = new HashSet<PlayerStats>(),
        Ratings = new HashSet<Rating>(),
        Engines = new HashSet<Engine>()
    };

    dbGame.PlayerStats.Add(new PlayerStats
    {
        Id = Guid.NewGuid(),
        GameId = dbGame.Id,
        PlayerCount = playerCount,
        Timestamp = timestamp
    });

    dbGame.Ratings.Add(new Rating
    {
        Id = Guid.NewGuid(),
        GameId = dbGame.Id,
        Score = rating.Score,
        ScoreDescription = rating.ScoreDescription,
        Timestamp = timestamp
    });

    using (var context = new EngineContext())
    {
        foreach (var igdbEngine in game.Engines)
        {
            var dbEngine = context.Engines.SingleOrDefault(e => e.IgdbId == igdbEngine.Id);
            if (dbEngine != null)
            {
                dbGame.Engines.Add(dbEngine);
            } else
            {
                dbGame.Engines.Add(new Engine
                {
                    Id = Guid.NewGuid(),
                    IgdbId = igdbEngine.Id,
                    Name = igdbEngine.Name
                });
            }
        }

        dbGames.Add(dbGame);

        context.Games.Add(dbGame);
        try
        {
            context.SaveChanges();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    i++;
}

Console.ReadKey();