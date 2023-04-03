
namespace EngineTool.Entities
{
    public class Rating
    {
        public Guid Id { get; set; }

        public int Score { get; set; }

        public string ScoreDescription { get; set; }

        public Guid GameId { get; set; }

        public Game Game { get; set; }
    }
}
