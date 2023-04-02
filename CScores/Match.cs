using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CScores
{
    internal class Match
    {
        private string date;
        private string time;
        private string id;
        private string url;
        private string homeTeamName;
        private string awayTeamName;
        private string score;
        private string status; //fin,         
        
        public string Date { get => date; set => date = value; }
        public string Time { get => time; set => time = value; }
        public string ID { get => id; set => id = value; }
        public string Url { get => url; set => url = value; }
        public string HomeTeamName { get => homeTeamName; set => homeTeamName = value; }
        public string AwayTeamName { get => awayTeamName; set => awayTeamName = value; }
        public string Score { get => score; set => score = value; }
        public string Status { get => status; set => status = value; }

        public Match() { }
        public Match(string id, string home, string away, string url)
        {
            ID = id;
            HomeTeamName = home;
            AwayTeamName = away;
            Url = url;
        }

        public override string ToString()
        {
            return string.Format($"[{ID} {Date} {Time}] {HomeTeamName} - {AwayTeamName} ({Status}) {Score}");
        }
    }
}
