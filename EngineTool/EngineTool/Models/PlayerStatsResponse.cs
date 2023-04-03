using System.Text.Json.Serialization;

namespace EngineTool.Models
{
    public class PlayerStatsResponse
    {
        [JsonPropertyName("response")]
        public PlayerStats PlayerStats { get; set; }
    }
}
