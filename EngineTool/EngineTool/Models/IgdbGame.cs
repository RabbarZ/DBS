using System.Text.Json.Serialization;

namespace EngineTool.Models
{
    public class IgdbGame
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("game_engines")]
        public ISet<IgdbEngine> Engines { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("websites")]
        public ISet<IgdbWebsite> Websites { get; set; }
    }
}
