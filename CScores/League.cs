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
        private string title;
        private string url;
        private string kindOfSport;
        private List<Match> matches;
        private List<Team> teams;
        public League() { }
        public League(string title, string url)
        {
            Title = title;
            Url = url;
        }
        public string Title { get => title; set => title = value; }
        public string Url { get => url; set => url = value; }
    }
}
