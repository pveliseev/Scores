using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CScores
{
    internal abstract class Parser
    {
        public abstract void GetMatches();
        public abstract void GetMatchStats();
    }

    internal class XScoresParser : Parser
    {
        public override void GetMatches()
        {
            
        }
        public override void GetMatchStats()
        {

        }
    }

    internal class FlashScoreParser : Parser
    {
        public override void GetMatches()
        {

        }
        public override void GetMatchStats()
        {

        }
    }
}
