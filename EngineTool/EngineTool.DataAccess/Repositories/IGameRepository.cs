using EngineTool.Entities;

namespace EngineTool.DataAccess.Repositories
{
    public interface IGameRepository
    {
        Game? GetByIgdbId(int igdbId);

        void Add(Game game);
    }
}
