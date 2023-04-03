
namespace EngineTool.Entities
{
    public class Engine
    {
        public Guid Id { get; set; }

        public string IdgbId { get; set; }

        public string Name { get; set; }

        public ISet<Game> Games { get; set; }
    }
}
