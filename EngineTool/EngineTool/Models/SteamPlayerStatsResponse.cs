using System.Text.Json.Serialization;

namespace EngineTool.Models
{
    public class SteamPlayerStatsResponse
    {
        [JsonPropertyName("response")]
        public SteamPlayerStats PlayerStats { get; set; }
    }
}
