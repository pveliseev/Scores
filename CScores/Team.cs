using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CScores
{
    internal class Team
    {
        private string name;
        private List<Game> games = null;

        public string Name { get => name; set => name = value; }

        public Team(string name)
        {
            Name = name;
            games = new List<Game>();
        }

        public override string ToString()
        {
            return string.Format($"{Name}");
        }
    }
}
