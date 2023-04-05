using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EngineTool.Entities
{
    public class Engine
    {
        public Guid Id { get; set; }

        public int IgdbId { get; set; }

        public string Name { get; set; }

        public ISet<Game> Games { get; set; }
    }
}
