using System.Text.Json.Serialization;

namespace EngineTool.Models;

public class IgdbGame
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("game_engines")]
    public required ISet<IgdbEngine> Engines { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("websites")]
    public required ISet<IgdbWebsite> Websites { get; set; }
}