using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineTool.Models
{
    public class PlayerStats
    {
        public Guid Id { get; set; }

        public int PlayerCount { get; set; }

        public DateTime Timestamp { get; set; }

        public Guid GameId { get; set; }

        public Game Game { get; set; }
    }
}
