using EngineTool.DataAccess.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngineTool.Entities
{
    [Table(nameof(Engine))]
    public class Engine : IEntity
    {
        public Guid Id { get; set; }

        public int IgdbId { get; set; }

        public required string Name { get; set; }

        public ISet<Game> Games { get; set; } = new HashSet<Game>();
    }
}
