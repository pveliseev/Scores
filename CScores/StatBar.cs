using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CScores
{
    internal class StatBar
    {
        public readonly string Title;
        public readonly double Value;
        public StatBar(string title, double value)
        {
            Title = title;
            Value = value;
        }

        public override string ToString()
        {
            return string.Format($"{Title}: {Value}");
        }
    }
}
