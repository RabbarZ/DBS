using EngineTool.Models;
using EngineTool.Services;

var igdbService = new IgdbService();
var steamService = new SteamService();
var timestamp = DateTime.UtcNow;

List<IgdbGame> games = await igdbService.GetGamesAsync(500);
List<EngineTool.Entities.Game> dbGames = new();
foreach (var game in games)
{
    string steamUrl = game.Websites.Single(w => w.Category == 13).Url;
    int steamId = 0;
    try
    {
        if (int.TryParse(steamUrl.Split('/')[4], out steamId))
        {
            throw new Exception("Error while parsing steam app id.");
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
        continue;
    }

    Console.WriteLine($"{game.Name} : {steamUrl}");

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

    var dbGame = new EngineTool.Entities.Game
    {
        Id = Guid.NewGuid(),
        Name = game.Name,
        PlayerStats = new HashSet<EngineTool.Entities.PlayerStats>(),
        Ratings = new HashSet<EngineTool.Entities.Rating>(),
    };

    dbGame.PlayerStats.Add(new EngineTool.Entities.PlayerStats
    {
        Id = Guid.NewGuid(),
        GameId = dbGame.Id,
        PlayerCount = playerCount,
        Timestamp = timestamp
    });

    dbGame.Ratings.Add(new EngineTool.Entities.Rating
    {
        Id = Guid.NewGuid(),
        GameId = dbGame.Id,
        Score = rating.Score,
        ScoreDescription = rating.ScoreDescription,
        Timestamp = timestamp
    });

    dbGames.Add(dbGame);
}

Console.ReadKey();