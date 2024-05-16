using System.ComponentModel.DataAnnotations.Schema;

namespace EngineTool.DataAccess.Entities;

[Table(nameof(PlayerStats))]
public class PlayerStats : IEntity
{
    public Guid Id { get; set; }

    public int PlayerCount { get; set; }

    public DateTime Timestamp { get; set; }

    public Guid GameId { get; set; }

    public Game? Game { get; set; }
}