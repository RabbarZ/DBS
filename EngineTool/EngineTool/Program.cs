using EngineTool.Config;
using EngineTool.Entities;
using EngineTool.Interfaces;
using EngineTool.Models;
using EngineTool.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EngineTool;

internal static class Program
{
    static IHostBuilder CreateHostBuilder(string[] args)
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        return Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddTransient<ISteamService, SteamService>();
                services.AddTransient<IIgdbService, IgdbService>();
                services.AddTransient<IDbService, DbService>();

                services.Configure<IgdbApiSettings>(config.GetSection("IgdbApi"));

                services.AddDbContext<EngineContext>(options =>
                {
                    options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
                });
            });
    }

    private static async Task Main(string[] args)
    {
        IHost host = CreateHostBuilder(args).Build();

        var igdbService = host.Services.GetRequiredService<IIgdbService>();
        var steamService = host.Services.GetRequiredService<ISteamService>();
        var dbService = host.Services.GetRequiredService<IDbService>();


        var timestamp = DateTime.UtcNow;

        dbService.EnsureDbExists();

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
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
            }

        }

        //using (var writer = new StreamWriter(appSettings.LogFilePath))
        //{
        //    writer.WriteLine("Successfully finished: " + DateTime.Now);
        //}
        //Console.WriteLine("Dabato");
    }
}