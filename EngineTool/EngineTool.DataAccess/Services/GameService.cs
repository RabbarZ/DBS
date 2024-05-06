using EngineTool.Entities;

namespace EngineTool.DataAccess.Services
{
    public class GameService : IGameService
    {
        private readonly IRepository<Game> repository;

        public GameService(IRepository<Game> repository)
        {
            this.repository = repository;
        }
        public Game? GetByIgdbId(int igdbId)
        {
            return this.repository.GetAll().SingleOrDefault(g => g.IgdbId == igdbId);
        }

        public void Add(Game game)
        {
            this.repository.Add(game);
        }
    }
}
