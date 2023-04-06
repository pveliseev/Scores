using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CScores
{
    internal class Player
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public Dictionary<string, List<StatBar>> PlayerStats { get; set; }
        public Player(string name)
        {
            Name = name;
        }
    }
}