using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CScores
{
    internal class StatBarTitles : Dictionary<Sport, HashSet<string>>
    {
        private static StatBarTitles _instance;
        private StatBarTitles() { }

        public static StatBarTitles Instance()
        {
            if (_instance == null)
                _instance = new StatBarTitles();
            return _instance;
        }

        //public void Load(string path)
        //{

        //}
    }
}
