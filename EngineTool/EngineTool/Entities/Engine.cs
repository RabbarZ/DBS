using System.ComponentModel.DataAnnotations;

namespace EngineTool.Entities
{
    public class Engine
    {
        public Guid Id { get; set; }

        [Key]
        public int IdgbId { get; set; }

        public string Name { get; set; }

        public ISet<Game> Games { get; set; }
    }
}
