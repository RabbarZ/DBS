using EngineTool.Entities;

namespace EngineTool.DataAccess.Repositories
{
    public interface IEngineRepository
    {
        Engine? GetByIgdbId(int igdbId);

        bool? GetContainsGame(Guid id, Guid gameId);

        void Add(Engine engine);

        void Update(Engine engine);
    }
}
