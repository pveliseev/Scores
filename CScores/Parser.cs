using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CScores
{
    internal abstract class Parser
    {
        public abstract void GetMatches(IWebDriver driver, League league);
        public abstract void GetGameStats(IWebDriver driver, League league);
    }

    internal class XScoresParser : Parser
    {
        public override void GetMatches(IWebDriver driver, League league)
        {

        }
        public override void GetGameStats(IWebDriver driver, League league)
        {

        }
    }

    internal class FlashScoreParser : Parser
    {
        public override void GetMatches(IWebDriver driver, League league)
        {
            driver.Navigate().GoToUrl(league.Url);

            //TODO поиск кнопки ПОКАЗАТЬ БОЛЬШЕ МАТЧЕЙ и щелкаем по ней покане прогрузится весь список

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(driver.PageSource);
            HtmlNodeCollection matchesNodes = doc.DocumentNode.SelectNodes("//div[contains(@class,'event__match event__match--static')]");

            league.Matches = new List<Match>();
            foreach (var match in matchesNodes)
            {
                string matchID = match.GetAttributeValue("id", "").Substring(4);
                string url = $"https://www.flashscore.com.ua/match/{matchID}/#/match-summary/match-statistics/0";
                string home = match.SelectSingleNode(".//div[contains(@class, 'event__participant--home')]").InnerText;
                string away = match.SelectSingleNode(".//div[contains(@class, 'event__participant--away')]").InnerText;

                league.Matches.Add(new Match(matchID, home, away, url));
            }
        }
        public override void GetGameStats(IWebDriver driver, League league)
        {
            int count = league.Matches.Count;
            for (int i = 20; i < 21; i++)
            {
                Match match = league.Matches[i];
                driver.Navigate().GoToUrl(match.Url);
                try
                {
                    //ожидаем подгрузки таблицы со статой
                    new WebDriverWait(driver, TimeSpan.FromSeconds(20)).Until(ExpectedConditions.ElementExists(By.ClassName("stat__row")));

                    //парсинг статы
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(driver.PageSource);

                    match.Date = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'duelParticipant__startTime')]").InnerText.Trim().Split(' ')[0].Trim();
                    match.Time = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'duelParticipant__startTime')]").InnerText.Trim().Split(' ')[1].Trim();
                    match.Score = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'detailScore__wrapper')]").InnerText;
                    match.Status = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'detailScore__status')]").InnerText.ToLower();

                    int homeScore = Convert.ToInt32(match.Score.Split('-')[0].Trim());
                    int awayScore = Convert.ToInt32(match.Score.Split('-')[1].Trim());

                    Team homeTeam = league.Teams.Find(t => t.Name == match.HomeTeamName);
                    Team awayTeam = league.Teams.Find(t => t.Name == match.AwayTeamName);

                    List<StatBar> homeStats = new List<StatBar>();
                    List<StatBar> awayStats = new List<StatBar>();
                    HtmlNodeCollection statsNodes = doc.DocumentNode.SelectNodes("//div[@class='stat__row']");
                    foreach (var stat in statsNodes)
                    {
                        string title = stat.SelectSingleNode(".//div[contains(@class, 'stat__categoryName')]").InnerText;
                        string homeValue = stat.SelectSingleNode(".//div[contains(@class, 'stat__homeValue')]").InnerText;
                        string awayValue = stat.SelectSingleNode(".//div[contains(@class, 'stat__awayValue')]").InnerText;

                        homeStats.Add(new StatBar(title, Convert.ToDouble(homeValue.Replace('.',','))));
                        awayStats.Add(new StatBar(title, Convert.ToDouble(awayValue.Replace('.', ','))));
                    }

                    homeTeam.Games.Add(new Game(true)
                    {
                        MatchID = match.ID,
                        Opponent = match.AwayTeamName,
                        Score = homeScore.ToString(),
                        Status = homeScore > awayScore ? "Win" : (homeScore < awayScore ? "Lose" : "Draw"),
                        Stats = homeStats
                    });

                    awayTeam.Games.Add(new Game(false)
                    {
                        MatchID = match.ID,
                        Opponent = match.HomeTeamName,
                        Score = awayScore.ToString(),
                        Status = awayScore > homeScore ? "Win" : (awayScore < homeScore ? "Lose" : "Draw"),
                        Stats = awayStats
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR: {ex.Message}");

                }
            }

        }
    }
}
