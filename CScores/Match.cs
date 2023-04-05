using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CScores
{
    internal class Match
    {
        public string ID { get; set; }
        public string Url { get; set; }
        public string League { get; set; }

        public Match() { }
        public Match(string id, string league, string url)
        {
            ID = id;
            League = league;
            Url = url;
        }
        public override string ToString()
        {
            return string.Format($"{ID} {League} {Url}");
        }
    }
}
