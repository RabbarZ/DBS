using EngineTool.Config;
using EngineTool.Entities;
using EngineTool.Interfaces;
using EngineTool.Models;
using EngineTool.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder appBuilder = Host.CreateApplicationBuilder(args);
appBuilder.Services.AddSingleton<ISteamService, SteamService>();
appBuilder.Services.AddSingleton<IIgdbService, IgdbService>();
using IHost host = appBuilder.Build();

var configBuilder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false);

IConfiguration config = configBuilder.Build();
var appSettings = config.Get<AppSettings>();

var igdbService = new IgdbService(appSettings, new HttpClient());
var steamService = new SteamService();
var timestamp = DateTime.UtcNow;

DbService.EnsureDbExists();

List<IgdbGame> games = await igdbService.GetGamesAsync(8500);

int i = 1;
foreach (var igdbGame in games)
{
    try
    {
        string steamUrl = igdbGame.Websites.Single(w => w.Category == 13).Url;
        int steamId = 0;
        try
        {
            if (!int.TryParse(steamUrl.Split('/')[4], out steamId))
            {
                Console.WriteLine("Error while parsing steam app id.");
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

        var playerCountResponse = await steamService.GetCurrentPlayerCountAsync(steamId);
        var ratingResponse = await steamService.GetRatingAsync(steamId);

        if (playerCountResponse.PlayerStats.Success != 1 || ratingResponse.Success != 1)
        {
            Console.WriteLine("Could not fetch data from Steam.");
            continue;
        }

        if (ratingResponse.Rating == null)
        {
            Console.WriteLine("Failure due to unknown error.");
            continue;
        }

        playerCount = playerCountResponse.PlayerStats.PlayerCount;
        rating = ratingResponse.Rating;

        var dbGame = DbService.GetGameByIdgbId(igdbGame.Id);
        if (dbGame == null)
        {
            dbGame = new Game
            {
                Id = Guid.NewGuid(),
                Name = igdbGame.Name,
                IgdbId = igdbGame.Id,
                SteamId = steamId
            };

            DbService.AddGame(dbGame);
        }

        DbService.AddPlayerStats(new PlayerStats
        {
            Id = Guid.NewGuid(),
            GameId = dbGame.Id,
            PlayerCount = playerCount,
            Timestamp = timestamp
        });

        DbService.AddRating(new Rating
        {
            Id = Guid.NewGuid(),
            GameId = dbGame.Id,
            Score = rating.Score,
            ScoreDescription = rating.ScoreDescription,
            Timestamp = timestamp
        });

        foreach (var igdbEngine in igdbGame.Engines)
        {
            var dbEngine = DbService.GetEngineByIdgbId(igdbEngine.Id);
            if (dbEngine == null)
            {
                dbEngine = new Engine
                {
                    Id = Guid.NewGuid(),
                    IgdbId = igdbEngine.Id,
                    Name = igdbEngine.Name
                };

                DbService.AddEngine(dbEngine);
            }

            dbGame = DbService.GetGameByIdgbId(dbGame.IgdbId);

            if (!DbService.GetEngineContainsGame(dbEngine.Id, dbGame.Id))
            {
                dbEngine.Games.Add(dbGame);
                DbService.UpdateEngine(dbEngine);
            }
        }

        i++;
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine(ex.ToString());
    }

}

using (var writer = new StreamWriter(appSettings.LogFilePath))
{
    writer.WriteLine("Successfully finished: " + DateTime.Now);
}
Console.WriteLine("Dabato");