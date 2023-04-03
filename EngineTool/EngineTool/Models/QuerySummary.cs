using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EngineTool.Models
{
    public class QuerySummary
    {
        [JsonPropertyName("query_summary")]
        public Rating Rating { get; set; }
    }
}
