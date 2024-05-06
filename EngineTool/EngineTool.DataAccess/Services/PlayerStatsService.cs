using EngineTool.Entities;

namespace EngineTool.DataAccess.Services
{
    public class PlayerStatsService : IPlayerStatsService
    {
        private readonly IRepository<PlayerStats> repository;

        public PlayerStatsService(IRepository<PlayerStats> repository)
        {
            this.repository = repository;
        }

        public void Add(PlayerStats playerStats)
        {
            this.repository.Add(playerStats);
        }
    }
}
