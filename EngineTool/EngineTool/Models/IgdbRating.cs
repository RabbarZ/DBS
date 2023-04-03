using System.Text.Json.Serialization;

namespace EngineTool.Models
{
    public class IgdbRating
    {
        [JsonPropertyName("review_score")]
        public int Score { get; set; }

        [JsonPropertyName("review_score_desc")]
        public string ScoreDescription { get; set; }
    }
}
