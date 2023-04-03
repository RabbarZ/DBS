using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EngineTool.Models
{
    internal class Rating
    {
        [JsonPropertyName("review_score")]
        public int Score { get; set; }

        [JsonPropertyName("review_score_desc")]
        public string ScoreDescription { get; set; }
    }
}
