using EngineTool.Entities;

namespace EngineTool.DataAccess.Services
{
    public interface IEngineService
    {
        Engine? GetByIgdbId(int igdbId);

        bool? GetContainsGame(Guid id, Guid gameId);

        void Add(Engine engine);

        void Update(Engine engine);
    }
}
