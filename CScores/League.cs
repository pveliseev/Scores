using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CScores
{
    internal enum Sport
    {
        Football,
        Hockey,
        Basketball,
        Baseball
    }
    internal class League
    {
        public Sport Sport { get; set; }
        public List<Page> Pages { get; set; }
        public List<Match> Matches { get; set; }
        public List<Game> Games { get; set; }
        public HashSet<string> StatBarTitles { get; set; }

        public League(Sport sport)
        {
            Sport = sport;
            Pages = new List<Page>();
        }
    }
}
