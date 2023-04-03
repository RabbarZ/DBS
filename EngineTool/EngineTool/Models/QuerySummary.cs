using System.Text.Json.Serialization;

namespace EngineTool.Models
{
    public class QuerySummary
    {
        [JsonPropertyName("success")]
        public int Success { get; set; }

        [JsonPropertyName("query_summary")]
        public Rating Rating { get; set; }

    }
}
