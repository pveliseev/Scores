using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CScores
{
    internal class Game
    {
        private List<StatBar> stats;
        public readonly bool isHome;
        private string opponent;
        private string status; //win, lose
        private string matchID;
        private string score;

        internal List<StatBar> Stats { get => stats; set => stats = value; }
        public string Opponent { get => opponent; set => opponent = value; }
        public string MatchID { get => matchID; set => matchID = value; }
        public string Score { get => score; set => score = value; }
        public string Status { get => status; set => status = value; }

        //private List<Player> players;

        public Game(bool isHome)
        {
            this.isHome = isHome;
        }
    }
}
