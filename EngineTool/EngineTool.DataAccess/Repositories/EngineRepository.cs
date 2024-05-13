using EngineTool.Entities;
using Microsoft.EntityFrameworkCore;

namespace EngineTool.DataAccess.Repositories
{
    public class EngineRepository(IRepository<Engine> repository) : IEngineRepository
    {
        private readonly IRepository<Engine> repository = repository;

        public Engine? GetByIgdbId(int igdbId)
        {
            return this.repository.GetAll().SingleOrDefault(e => e.IgdbId == igdbId);
        }

        public bool? GetContainsGame(Guid id, Guid gameId)
        {
            return this.repository.GetAll().Include(e => e.Games).SingleOrDefault(e => e.Id == id)?.Games.Any(g => g.Id == gameId);
        }

        public void Add(Engine engine)
        {
            this.repository.Add(engine);
        }

        public void Update(Engine engine)
        {
            this.repository.Update(engine);
        }
    }
}
