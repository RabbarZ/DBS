using System.ComponentModel.DataAnnotations.Schema;

namespace EngineTool.Entities
{
    [Table(nameof(Game))]
    public class Game
    {
        public Guid Id { get; set; }

        public int IgdbId { get; set; }

        public int SteamId { get; set; }

        public required string Name { get; set; }

        public ISet<Engine> Engines { get; set; } = new HashSet<Engine>();

        public ISet<Rating> Ratings { get; set; } = new HashSet<Rating>();

        public ISet<PlayerStats> PlayerStats { get; set; } = new HashSet<PlayerStats>();
    }
}
