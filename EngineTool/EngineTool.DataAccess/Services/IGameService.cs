using EngineTool.Entities;

namespace EngineTool.DataAccess.Services
{
    public interface IGameService
    {
        Game? GetByIgdbId(int igdbId);

        void Add(Game game);
    }
}
