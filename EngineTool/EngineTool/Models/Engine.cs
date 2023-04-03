using System.Text.Json.Serialization;

namespace EngineTool.Models
{
    public class Engine
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
