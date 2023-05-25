using EngineTool.Entities;
using EngineTool.Models;
using EngineTool.Services;

var igdbService = new IgdbService();
var steamService = new SteamService();
var dbService = new DbService();
var timestamp = DateTime.UtcNow;

List<IgdbGame> games = await igdbService.GetGamesAsync(8500);
List<Game> dbGames = new();

int i = 1;
foreach (var igdbGame in games)
{
    string steamUrl = igdbGame.Websites.Single(w => w.Category == 13).Url;
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

    Console.WriteLine($"{i} : {igdbGame.Name} : {steamUrl}");

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

    var dbGame = dbService.GetGameByIdgbId(igdbGame.Id);
    if (dbGame == null)
    {
        dbGame = new Game
        {
            Id = Guid.NewGuid(),
            Name = igdbGame.Name,
            IgdbId = igdbGame.Id,
            SteamId = steamId
        };

        dbService.AddGame(dbGame);
    }

    dbService.AddPlayerStats(new PlayerStats
    {
        Id = Guid.NewGuid(),
        GameId = dbGame.Id,
        PlayerCount = playerCount,
        Timestamp = timestamp
    });

    dbService.AddRating(new Rating
    {
        Id = Guid.NewGuid(),
        GameId = dbGame.Id,
        Score = rating.Score,
        ScoreDescription = rating.ScoreDescription,
        Timestamp = timestamp
    });

    foreach (var igdbEngine in igdbGame.Engines)
    {
        var dbEngine = dbService.GetEngineByIdgbId(igdbEngine.Id);
        if (dbEngine == null)
        {
            dbEngine = new Engine
            {
                Id = Guid.NewGuid(),
                IgdbId = igdbEngine.Id,
                Name = igdbEngine.Name
            };

            dbService.AddEngine(dbEngine);
        }

        dbGame = dbService.GetGameByIdgbId(dbGame.IgdbId);

        if (!dbService.GetEngineContainsGame(dbEngine.Id, dbGame.Id))
        {
            dbEngine.Games.Add(dbGame);
            dbService.UpdateEngine(dbEngine);
        }
    }

    i++;
}

Console.ReadKey();