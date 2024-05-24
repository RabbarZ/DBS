﻿using System.ComponentModel.DataAnnotations.Schema;

namespace EngineTool.Entities
{
    [Table(nameof(Rating))]
    public class Rating
    {
        public Guid Id { get; set; }

        public int Score { get; set; }

        public string ScoreDescription { get; set; }

        public Guid GameId { get; set; }

        public Game Game { get; set; }

        public DateTime Timestamp { get; set; }
    }
}