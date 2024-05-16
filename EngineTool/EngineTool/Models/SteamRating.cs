using System.Text.Json.Serialization;

namespace EngineTool.Models;

public class SteamRating
{
    [JsonPropertyName("review_score")]
    public int Score { get; set; }

    [JsonPropertyName("review_score_desc")]
    public required string ScoreDescription { get; set; }
}