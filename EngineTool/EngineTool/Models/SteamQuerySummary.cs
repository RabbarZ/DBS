using System.Text.Json.Serialization;

namespace EngineTool.Models
{
    public class SteamQuerySummary
    {
        [JsonPropertyName("success")]
        public int Success { get; set; }

        [JsonPropertyName("query_summary")]
        public IgdbRating Rating { get; set; }
    }
}
