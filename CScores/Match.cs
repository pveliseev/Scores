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
        private Team home;
        private Team away;
        private string score;
        private string status; //fin, 
        

        public Match() { }

        public string Date { get => date; set => date = value; }
        public string Time { get => time; set => time = value; }
        public string Id { get => id; set => id = value; }
        public string Url { get => url; set => url = value; }
        public Team Home { get => home; set => home = value; }
        public Team Away { get => away; set => away = value; }
        public string Score { get => score; set => score = value; }
        public string Status { get => status; set => status = value; }
    }
}
