using System.Text.Json.Serialization;

namespace EngineTool.Models
{
    public class Website
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("category")]
        public int Category { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
