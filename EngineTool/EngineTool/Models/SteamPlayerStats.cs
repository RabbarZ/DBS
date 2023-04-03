using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EngineTool.Models
{
    public class SteamPlayerStats
    {
        [JsonPropertyName("player_count")]
        public int PlayerCount { get; set; }
    }
}
