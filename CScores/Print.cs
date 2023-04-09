﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CScores
{
    internal static class Print
    {
        public static void Baseball(League league)
        {
            //заголовок таблицы игр
            StringBuilder gamesTable = new StringBuilder();
            gamesTable.Append("Liga;MatchID;Date;Time;Status;Owner;Form;Score;IsHome;Rival;Url;");
            foreach (var item in league.GameStatBarTitles)
            {
                gamesTable.Append($"{item};");
            }
            gamesTable.AppendLine();

            if (league.Games != null)
            {
                //данные таблицы игр
                foreach (var game in league.Games.Cast<TeamGame>())
                {
                    if (game.TeamStats != null)
                    {
                        //формируем строки статистики
                        StringBuilder teamStats = new StringBuilder();
                        foreach (var item in league.GameStatBarTitles)
                        {
                            if (game.TeamStats["матч"].Exists(b => b.Title == item))
                            {
                                teamStats.Append(game.TeamStats["матч"].Find(b => b.Title == item).Value + ";");
                            }
                            else
                            {
                                teamStats.Append(";");
                            }
                        }
                        gamesTable.AppendLine($"{game.LeagueTitle};{game.MatchID};{game.Date};{game.Time};{game.Status};{game.Owner.Name};{game.Form};{game.Score};{game.IsHome};{game.Rival.Name};{game.URL};{teamStats}");
                    }
                }
            }
            //запись в фаил статистики игр
            using (StreamWriter sw = new StreamWriter("BaseballGames.txt"))
            {
                sw.Write(gamesTable.ToString());
            }

            //==================================================
            //заголовок таблицы игроков
            StringBuilder playersTable = new StringBuilder();
            playersTable.Append("Liga;MatchID;Date;Time;Owner;Url;Player;Role;");
            foreach (var item in league.PlayerStatBarTitles)
            {
                playersTable.Append($"{item};");
            }
            playersTable.AppendLine();

            if (league.Games != null)
            {
                //данные таблицы игр
                foreach (var game in league.Games.Cast<TeamGame>())
                {
                    if (game.Players != null)
                    {
                        //формируем строки статистики
                        foreach (var player in game.Players)
                        {
                            StringBuilder playerStats = new StringBuilder();
                            foreach (var item in league.PlayerStatBarTitles)
                            {
                                if (player.PlayerStats.Exists(b => b.Title == item))
                                {
                                    playerStats.Append(player.PlayerStats.Find(b => b.Title == item).Value + ";");
                                }
                                else
                                {
                                    playerStats.Append(";");
                                }
                            }
                            playersTable.AppendLine($"{game.LeagueTitle};{game.MatchID};{game.Date};{game.Time};{game.Owner.Name};{game.URL};{player.Name};{player.Role};{playerStats}");
                        }
                    }
                }
            }
            //запись в фаил статистики игроков
            using (StreamWriter sw = new StreamWriter("BaseballPlayers.txt"))
            {
                sw.Write(playersTable.ToString());
            }
        }
    }
}