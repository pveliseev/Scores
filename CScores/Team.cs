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
        //private string url;

        private List<Game> games;        
        public Team(string name)
        {
            this.name = name;
        }
    }
}
