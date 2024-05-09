using EngineTool.Config;
using EngineTool.DataAccess;
using EngineTool.DataAccess.Services;
using EngineTool.Entities;
using EngineTool.Interfaces;
using EngineTool.Models;
using EngineTool.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

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
                services.Configure<IgdbApiSettings>(config.GetSection("IgdbApi"));

                services.AddTransient<IIgdbService, IgdbService>();
                services.AddHttpClient<IIgdbService, IgdbService>((serviceProvider, httpClient) =>
                {
                    IgdbApiSettings apiSettings = serviceProvider.GetRequiredService<IOptions<IgdbApiSettings>>().Value;
                    httpClient.DefaultRequestHeaders.Add("Client-ID", apiSettings.ClientId);
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiSettings.BearerToken}");
                });

                services.AddTransient<ISteamService, SteamService>();

                services.AddTransient<IRepository<Game>, Repository<Game>>();
                services.AddTransient<IRepository<Engine>, Repository<Engine>>();
                services.AddTransient<IRepository<PlayerStats>, Repository<PlayerStats>>();
                services.AddTransient<IRepository<Rating>, Repository<Rating>>();

                services.AddTransient<IGameService, GameService>();
                services.AddTransient<IEngineService, EngineService>();
                services.AddTransient<IPlayerStatsService, PlayerStatsService>();
                services.AddTransient<IRatingService, RatingService>();

                services.AddDbContext<IEngineContext, EngineContext>(options =>
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

        var gameService = host.Services.GetRequiredService<IGameService>();
        var engineService = host.Services.GetRequiredService<IEngineService>();
        var playerStatsService = host.Services.GetRequiredService<IPlayerStatsService>();
        var ratingService = host.Services.GetRequiredService<IRatingService>();

        var timestamp = DateTime.UtcNow;

        var games = igdbService.GetGamesAsync();

        await foreach (var igdbGame in games)
        {
            //try
            //{
                await ProcessGameAsync(igdbGame, igdbService, steamService, gameService, engineService, playerStatsService, ratingService, timestamp);
            //}
            //catch (Exception ex)
            //{
            //        Console.WriteLine(ex.ToString());
            //    Console.ReadKey();
            //}
        }
    }

    private static async Task ProcessGameAsync(
        IgdbGame igdbGame,
        IIgdbService igdbService,
        ISteamService steamService,
        IGameService gameService,
        IEngineService engineService,
        IPlayerStatsService playerStatsService,
        IRatingService ratingService,
        DateTime timestamp)
    {
        int? steamId = igdbService.GetSteamId(igdbGame);
        if (steamId == null)
        {
            Console.WriteLine("Could not fetch Steam ID from IGDB.");
            return;
        }

        var playerCount = await steamService.GetCurrentPlayerCountAsync(steamId.Value);
        if (playerCount == null)
        {
            Console.WriteLine("Could not fetch current player count from Steam.");
            return;
        }

        var rating = await steamService.GetRatingAsync(steamId.Value);
        if (rating == null)
        {
            Console.WriteLine("Could not fetch rating from Steam.");
            return;
        }

        var dbGame = GetOrCreateGame(igdbGame, steamId.Value, gameService);
        if (dbGame == null)
        {
            return;
        }

        playerStatsService.Add(new PlayerStats
        {
            Id = Guid.NewGuid(),
            GameId = dbGame.Id,
            PlayerCount = playerCount.Value,
            Timestamp = timestamp
        });

        ratingService.Add(new Rating
        {
            Id = Guid.NewGuid(),
            GameId = dbGame.Id,
            Score = rating.Score,
            ScoreDescription = rating.ScoreDescription,
            Timestamp = timestamp
        });

        foreach (var igdbEngine in igdbGame.Engines)
        {
            ProcessEngine(igdbEngine, dbGame, engineService);
        }
    }

    private static Game GetOrCreateGame(
        IgdbGame igdbGame,
        int steamId,
        IGameService gameService)
    {
        var dbGame = gameService.GetByIgdbId(igdbGame.Id);
        if (dbGame == null)
        {
            dbGame = new Game
            {
                Id = Guid.NewGuid(),
                Name = igdbGame.Name,
                IgdbId = igdbGame.Id,
                SteamId = steamId
            };

            gameService.Add(dbGame);
        }

        return dbGame;
    }

    private static void ProcessEngine(
        IgdbEngine igdbEngine,
        Game dbGame,
        IEngineService engineService)
    {
        var dbEngine = engineService.GetByIgdbId(igdbEngine.Id);
        if (dbEngine == null)
        {
            dbEngine = new Engine
            {
                Id = Guid.NewGuid(),
                IgdbId = igdbEngine.Id,
                Name = igdbEngine.Name
            };

            engineService.Add(dbEngine);
        }

        var containsGame = engineService.GetContainsGame(dbEngine.Id, dbGame.Id);

        if (containsGame != null && !containsGame.Value)
        {
            dbEngine.Games.Add(dbGame);
            engineService.Update(dbEngine);
        }
    }
}