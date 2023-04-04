using System.ComponentModel.DataAnnotations;

namespace EngineTool.Entities
{
    public class Game
    {
        public Guid Id { get; set; }

        [Key]
        public int IgdbId { get; set; }

        [Key]
        public int SteamId { get; set; }

        public string Name { get; set; }

        public ISet<Engine> Engines { get; set; }

        public ISet<Rating> Ratings { get; set; }

        public ISet<PlayerStats> PlayerStats { get; set; }
    }
}
