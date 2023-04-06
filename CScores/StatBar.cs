using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CScores
{
    internal struct StatBar
    {
        public string Title { get; set; }
        public double Value { get; set; }

        public StatBar(string title, double value)
        {
            Title = title;
            Value = value;
        }
        public override string ToString()
        {
            return string.Format($"{Title} {Value}");
        }
    }
}
