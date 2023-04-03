using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineTool.Models
{
    public class Rating
    {
        public Guid Id { get; set; }

        public int Score { get; set; }

        public string ScoreDescription { get; set; }

        public Guid GameId { get; set; }

        public Game Game { get; set; }
    }
}
