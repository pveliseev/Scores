using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CScores
{
    internal static class Print
    {
        public static void Table(List<League> leagues, StatBarTitles template)
        {
            foreach (var sportStatBar in template)
            {
                //заголовок таблицы
                StringBuilder row = new StringBuilder();
                row.Append("Sport;Liga;Data;Time;Team;IsHome;Score;Status;Opponent;Url;");
                foreach(var statBarTitle in sportStatBar.Value)
                {
                    row.Append($"{statBarTitle};");
                }
                row.AppendLine();

                //данные таблицы
                
                
            }



        }
    }
}
