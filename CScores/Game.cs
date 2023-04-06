using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CScores
{
    internal class Game
    {
        public string MatchID { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Status { get; set; } //final
        public string Form { get; set; } //win, lose, draw
        public bool IsHome { get; set; }
        public string LeagueTitle { get; set; }
        public int Score { get; set; }
        public string URL { get; set; }
        public Dictionary<string, List<StatBar>> TeamStats { get; set; } //subTabs => Stats
        public Game() { }

    }

    internal class TeamGame : Game
    {
        public Team Owner { get; set; }
        public Team Rival { get; set; }

        public override string ToString()
        {            
            return string.Format($"{Owner.Name} {Form}");
        }
    }

    internal class IndividualGame : Game
    {
        public Player Owner { get; set; }
        public Player Rival { get; set; }
    }
}
