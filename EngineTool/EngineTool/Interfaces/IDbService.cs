using EngineTool.Entities;

namespace EngineTool.Interfaces
{
    public interface IDbService
    {
        void EnsureDbExists();

        Game GetGameByIdgbId(int igdbId);

        Engine GetEngineByIdgbId(int igdbId);

        bool GetEngineContainsGame(Guid engineId, Guid gameId);

        Engine AddEngine(Engine engine);

        Engine UpdateEngine(Engine engine);

        Game AddGame(Game game);

        PlayerStats AddPlayerStats(PlayerStats playerStats);

        Rating AddRating(Rating rating);
    }
}
