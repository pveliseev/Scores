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
        private string status; //win, lose
        //private string url;
        //private List<Player> players;
        private List<StatBar> statBars;
        public readonly bool isHome;
        public Team(string name, string status, bool isHome)
        {
            this.name = name;
            this.status = status;
            this.isHome = isHome;
        }
    }
}
