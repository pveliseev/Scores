using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CScores
{
    internal class Team
    {
        public string Name { get; set; }
        public List<Player> Players { get; set; }

        public Team(string name)
        {
            Name = name;
        }
    }
}
