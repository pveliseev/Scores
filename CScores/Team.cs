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
        internal List<Game> Games { get => games; set => games = value; }

        public Team(string name)
        {
            Name = name;
            Games = new List<Game>();
        }

        public override string ToString()
        {
            return string.Format($"{Name}");
        }
    }
}
