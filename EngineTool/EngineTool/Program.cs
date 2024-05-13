using EngineTool.Config;
using EngineTool.DataAccess;
using EngineTool.DataAccess.Repositories;
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

                services.AddTransient<IGameRepository, GameRepository>();
                services.AddTransient<IEngineRepository, EngineRepository>();
                services.AddTransient<IPlayerStatsRepository, PlayerStatsRepository>();
                services.AddTransient<IRatingRepository, RatingRepository>();

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

        var gameService = host.Services.GetRequiredService<IGameRepository>();
        var engineService = host.Services.GetRequiredService<IEngineRepository>();
        var playerStatsService = host.Services.GetRequiredService<IPlayerStatsRepository>();
        var ratingService = host.Services.GetRequiredService<IRatingRepository>();

        var timestamp = DateTime.UtcNow;

        var games = igdbService.GetGamesAsync();

        await foreach (var igdbGame in games)
        {
            await ProcessGameAsync(igdbGame, igdbService, steamService, gameService, engineService, playerStatsService, ratingService, timestamp);
        }
    }

    private static async Task ProcessGameAsync(
        IgdbGame igdbGame,
        IIgdbService igdbService,
        ISteamService steamService,
        IGameRepository gameRepository,
        IEngineRepository engineRepository,
        IPlayerStatsRepository playerStatsRepository,
        IRatingRepository ratingRepository,
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

        var dbGame = GetOrCreateGame(igdbGame, steamId.Value, gameRepository);
        if (dbGame == null)
        {
            return;
        }

        playerStatsRepository.Add(new PlayerStats
        {
            Id = Guid.NewGuid(),
            GameId = dbGame.Id,
            PlayerCount = playerCount.Value,
            Timestamp = timestamp
        });

        ratingRepository.Add(new Rating
        {
            Id = Guid.NewGuid(),
            GameId = dbGame.Id,
            Score = rating.Score,
            ScoreDescription = rating.ScoreDescription,
            Timestamp = timestamp
        });

        foreach (var igdbEngine in igdbGame.Engines)
        {
            ProcessEngine(igdbEngine, dbGame, engineRepository);
        }
    }

    private static Game GetOrCreateGame(
        IgdbGame igdbGame,
        int steamId,
        IGameRepository gameRepository)
    {
        var dbGame = gameRepository.GetByIgdbId(igdbGame.Id);
        if (dbGame == null)
        {
            dbGame = new Game
            {
                Id = Guid.NewGuid(),
                Name = igdbGame.Name,
                IgdbId = igdbGame.Id,
                SteamId = steamId
            };

            gameRepository.Add(dbGame);
        }

        return dbGame;
    }

    private static void ProcessEngine(
        IgdbEngine igdbEngine,
        Game dbGame,
        IEngineRepository engineService)
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