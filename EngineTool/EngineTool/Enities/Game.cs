
namespace EngineTool.Entities
{
    public class Game
    {
        public Guid Id { get; set; }

        public string SteamId { get; set; }

        public string Name { get; set; }

        public ISet<Engine> Engines { get; set; }

        public ISet<Rating> Ratings { get; set; }

        public ISet<PlayerStats> PlayerStats { get; set; }
    }
}
