using EngineTool.DataAccess.Entities;
using EngineTool.DataAccess.Interfaces;
using EngineTool.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EngineTool.DataAccess.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IRepository<Game>, Repository<Game>>();
            serviceCollection.AddTransient<IRepository<Engine>, Repository<Engine>>();
            serviceCollection.AddTransient<IRepository<PlayerStats>, Repository<PlayerStats>>();
            serviceCollection.AddTransient<IRepository<Rating>, Repository<Rating>>();

            serviceCollection.AddTransient<IGameRepository, GameRepository>();
            serviceCollection.AddTransient<IEngineRepository, EngineRepository>();
            serviceCollection.AddTransient<IPlayerStatsRepository, PlayerStatsRepository>();
            serviceCollection.AddTransient<IRatingRepository, RatingRepository>();
        }

        public static void AddEngineContext(this IServiceCollection serviceCollection, Action<DbContextOptionsBuilder> optionsAction)
        {
            serviceCollection.AddDbContext<IEngineContext, EngineContext>(optionsAction);
        }
    }
}
