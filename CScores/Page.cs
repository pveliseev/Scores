using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CScores
{
    internal struct Page
    {
        public string Title { get; set; }
        public string Url { get; set; }

        public Page(string title, string url)
        {
            Title = title;
            Url = url;
        }
        public override string ToString()
        {
            return string.Format($"{Title} {Url}");
        }
    }
}
