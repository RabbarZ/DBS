
using EngineTool.Models;
using EngineTool.Services;

var igdbService = new IgdbService();
var steamService = new SteamService();

List<IgdbGame> games = await igdbService.GetGamesAsync(8000);
List<EngineTool.Entities.Game> dbGames = new List<EngineTool.Entities.Game>();
foreach (var game in games)
{

    string steamUrl = game.Websites.Single(w => w.Category == 13).Url;
    string steamId = string.Empty;
    try
    {
        steamId = steamUrl.Split('/')[4];
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
        Timestamp = DateTime.UtcNow
    });

    dbGame.Ratings.Add(new EngineTool.Entities.Rating
    {
        Id = Guid.NewGuid(),
        GameId = dbGame.Id,
        Score = rating.Score,
        ScoreDescription = rating.ScoreDescription
    });

    dbGames.Add(dbGame);
}

Console.ReadKey();