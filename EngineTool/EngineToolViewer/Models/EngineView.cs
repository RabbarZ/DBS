using EngineTool.Entities;

namespace EngineToolViewer.Models
{
    public class EngineView
    {
        public string Name { get; set; }

        public int GamesCount { get; set; }

        public string GameNames { get; set; }

        public double AveragePlayerCount { get; set; }

        public double AverageRating { get; set; }

        public override string ToString()
        {
            return "Engine: " + this.Name + "\n\t Games count: " + this.GamesCount + "\n\t Game names: " + this.GameNames + "\n\t Player count: " + this.AveragePlayerCount + "\n\t rating: " + this.AverageRating;
        }
    }
}
