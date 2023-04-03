
namespace EngineTool.Entities
{
    public class PlayerStats
    {
        public Guid Id { get; set; }

        public int PlayerCount { get; set; }

        public DateTime Timestamp { get; set; }

        public Guid GameId { get; set; }

        public Game Game { get; set; }
    }
}
