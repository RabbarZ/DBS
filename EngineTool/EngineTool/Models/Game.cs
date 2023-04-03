using System.Text.Json.Serialization;

namespace EngineTool.Models
{
    public class Game
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("game_engines")]
        public ISet<Engine> Engines { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("websites")]
        public ISet<Website> Websites { get; set; }
    }
}
