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
        public List<StatBar> PlayerStats { get; set; }
        public Player(string name, string role)
        {
            Name = name;
            Role = role;
        }

        public override string ToString()
        {
            return string.Format($"{Name} {Role}  Stats: {PlayerStats?.Count}");
        }
    }
}