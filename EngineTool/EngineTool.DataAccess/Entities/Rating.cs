using System.ComponentModel.DataAnnotations.Schema;

namespace EngineTool.DataAccess.Entities;

[Table(nameof(Rating))]
public class Rating : IEntity
{
    public Guid Id { get; set; }

    public int Score { get; set; }

    public required string ScoreDescription { get; set; }

    public Guid GameId { get; set; }

    public Game? Game { get; set; }

    public DateTime Timestamp { get; set; }
}