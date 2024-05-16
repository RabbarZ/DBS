using System.Text.Json.Serialization;

namespace EngineTool.Models;

public class IgdbEngine
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }
}