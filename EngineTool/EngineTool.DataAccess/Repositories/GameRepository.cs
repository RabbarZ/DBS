using EngineTool.Entities;

namespace EngineTool.DataAccess.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly IRepository<Game> repository;

        public GameRepository(IRepository<Game> repository)
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
