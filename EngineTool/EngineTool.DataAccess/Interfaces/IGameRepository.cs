using EngineTool.DataAccess.Entities;

namespace EngineTool.DataAccess.Interfaces;

public interface IGameRepository : IRepository<Game>
{
    Game? GetByIgdbId(int igdbId);
}