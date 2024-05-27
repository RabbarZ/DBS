using EngineTool.DataAccess.Entities;

namespace EngineTool.DataAccess.Interfaces;

public interface IEngineRepository : IRepository<Engine>
{
    Engine? GetByIgdbId(int igdbId);

    bool? GetContainsGame(Guid id, Guid gameId);
}