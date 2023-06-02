namespace EngineToolViewer.Models
{
    public class GameView
    {
        public string Name { get; set; }

        public int EnginesCount { get; set; }

        public string EngineNames { get; set; }

        public double? AveragePlayerCount { get; set; }

        public double? AverageRating { get; set; }

        public override string ToString()
        {
            return "Game: " + this.Name + "\n\t Engines count: " + this.EnginesCount + "\n\t Engine names: " + this.EngineNames + "\n\t Player count: " + this.AveragePlayerCount + "\n\t rating: " + this.AverageRating;
        }
    }
}
