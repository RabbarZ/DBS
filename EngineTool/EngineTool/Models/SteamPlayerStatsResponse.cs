using System.Text.Json.Serialization;

namespace EngineTool.Models;

public class SteamPlayerStatsResponse
{
    [JsonPropertyName("response")]
    public required SteamPlayerStats PlayerStats { get; set; }
}