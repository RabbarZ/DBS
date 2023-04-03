using System.Text.Json.Serialization;

namespace EngineTool.Models
{
    public class PlayerStats
    {
        [JsonPropertyName("result")]
        public int Success { get; set; }

        [JsonPropertyName("player_count")]
        public int PlayerCount { get; set; }
    }
}
