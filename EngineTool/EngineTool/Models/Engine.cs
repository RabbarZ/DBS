using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineTool.Models
{
    public class Engine
    {
        public Guid Id { get; set; }

        public string IdgbId { get; set; }

        public string Name { get; set; }

        public ISet<Game> Games { get; set; }
    }
}
