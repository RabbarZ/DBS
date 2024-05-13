using EngineTool.Entities;

namespace EngineTool.DataAccess.Repositories
{
    public class PlayerStatsRepository : IPlayerStatsRepository
    {
        private readonly IRepository<PlayerStats> repository;

        public PlayerStatsRepository(IRepository<PlayerStats> repository)
        {
            this.repository = repository;
        }

        public void Add(PlayerStats playerStats)
        {
            this.repository.Add(playerStats);
        }
    }
}
