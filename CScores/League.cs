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
        private Sport kindOfSport;
        private List<Match> matches = null;
        private List<Team> teams = null;
        //public League() { }

        public string Title { get => title; set => title = value; }
        public string Url { get => url; set => url = value; }
        public List<Match> Matches { get => matches; set => matches = value; }
        public List<Team> Teams { get => teams; set => teams = value; }
        public Sport KindOfSport { get => kindOfSport; set => kindOfSport = value; }

        public League(Sport kindOfSport, string title, string url)
        {
            KindOfSport = kindOfSport;
            Title = title;
            Url = url;
        }

        public void TeamsInit()
        {
            if (Matches != null)
            {
                Teams = new List<Team>();
                foreach (Match match in matches)
                {
                    if(!Teams.Exists(t => t.Name == match.HomeTeamName))
                    {
                        Teams.Add(new Team(match.HomeTeamName));
                    }

                    if (!Teams.Exists(t => t.Name == match.AwayTeamName))
                    {
                        Teams.Add(new Team(match.AwayTeamName));
                    }
                }
            }
        }

        public override string ToString()
        {
            return string.Format($"{KindOfSport}: {Title}");
        }
    }
}
