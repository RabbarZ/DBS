using EngineTool.DataAccess.Entities;
using EngineTool.DataAccess.Extensions;
using EngineTool.DataAccess.Interfaces;
using EngineTool.Extensions;
using EngineTool.Interfaces;
using EngineTool.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EngineTool;

internal static class Program
{
    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddConfiguration();
                var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

                services.AddServices();

                services.AddRepositories();

                services.AddEngineContext(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
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

        DateTime timestamp = DateTime.UtcNow;

        IAsyncEnumerable<IgdbGame> games = igdbService.GetGamesAsync();

        await foreach (IgdbGame igdbGame in games)
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

        int? playerCount = await steamService.GetCurrentPlayerCountAsync(steamId.Value);
        if (playerCount == null)
        {
            Console.WriteLine("Could not fetch current player count from Steam.");
            return;
        }

        SteamRating? rating = await steamService.GetRatingAsync(steamId.Value);
        if (rating == null)
        {
            Console.WriteLine("Could not fetch rating from Steam.");
            return;
        }

        Game dbGame = GetOrCreateGame(igdbGame, steamId.Value, gameRepository);

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

        foreach (IgdbEngine igdbEngine in igdbGame.Engines)
        {
            ProcessEngine(igdbEngine, dbGame, engineRepository);
        }
    }

    private static Game GetOrCreateGame(
        IgdbGame igdbGame,
        int steamId,
        IGameRepository gameRepository)
    {
        Game? dbGame = gameRepository.GetByIgdbId(igdbGame.Id);
        if (dbGame != null)
        {
            return dbGame;
        }

        dbGame = new Game
        {
            Id = Guid.NewGuid(),
            Name = igdbGame.Name,
            IgdbId = igdbGame.Id,
            SteamId = steamId
        };

        gameRepository.Add(dbGame);

        return dbGame;
    }

    private static void ProcessEngine(
        IgdbEngine igdbEngine,
        Game dbGame,
        IEngineRepository engineService)
    {
        Engine? dbEngine = engineService.GetByIgdbId(igdbEngine.Id);
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

        bool? containsGame = engineService.GetContainsGame(dbEngine.Id, dbGame.Id);

        if (containsGame == null || containsGame.Value)
        {
            return;
        }

        dbEngine.Games.Add(dbGame);
        engineService.Update(dbEngine);
    }
}