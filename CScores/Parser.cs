using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace CScores
{
    internal abstract class Parser
    {
        protected const string WIN = "WIN";
        protected string LOSE = "LOSE";
        protected const string DRAW = "DRAW";

        public abstract void GetMatches(IWebDriver driver, League league);
        public abstract void GetTeamGames(IWebDriver driver, League league);
    }

    internal class XScoresParser : Parser
    {
        public override void GetMatches(IWebDriver driver, League league)
        {

        }
        public override void GetTeamGames(IWebDriver driver, League league)
        {

        }
    }

    internal class FlashScoreParser : Parser
    {
        public override void GetMatches(IWebDriver driver, League league)
        {
            league.Matches = new List<Match>();
            foreach (var page in league.Pages)
            {
                driver.Navigate().GoToUrl(page.Url);

                //поиск кнопки ПОКАЗАТЬ БОЛЬШЕ МАТЧЕЙ и щелкаем по ней покане прогрузится весь список
                //while (true)
                //{
                //    try
                //    {
                //        Thread.Sleep(5000);
                //        new Actions(driver).KeyDown(Keys.End).Perform();                    
                //        driver.FindElement(By.XPath("//a[contains(@class, 'event__more event__more--static')]")).Click();
                //    }
                //    catch { break; }
                //}

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(driver.PageSource);
                HtmlNodeCollection matchesNodes = doc.DocumentNode.SelectNodes("//div[contains(@class,'event__match event__match--static')]");

                var tmp = new List<Match>();
                foreach (var match in matchesNodes)
                {
                    string id = match.GetAttributeValue("id", "");
                    string url = $"https://www.flashscore.com.ua/match/{id.Substring(4)}/#/match-summary/match-statistics/0";
                    //string home = match.SelectSingleNode(".//div[contains(@class, 'event__participant--home')]").InnerText;
                    //string away = match.SelectSingleNode(".//div[contains(@class, 'event__participant--away')]").InnerText;

                    tmp.Add(new Match(id, page.Title, url));
                }

                league.Matches.AddRange(tmp);
            }
        }
        public override void GetTeamGames(IWebDriver driver, League league)
        {
            //инициализация коллекций которые будут здесь заполнены
            league.TeamNames = new HashSet<string>();
            league.StatBarTitles = new HashSet<string>();
            var games = new List<TeamGame>();

            int cnt = league.Matches.Count;
            for (int i = 0; i < 3; i++)
            {
                Match match = league.Matches[i];
                driver.Navigate().GoToUrl(match.Url);

                try
                {
                    //ожидаем подгрузки таблицы со статой
                    new WebDriverWait(driver, TimeSpan.FromSeconds(20)).Until(ExpectedConditions.ElementExists(By.ClassName("stat__row")));
                    Thread.Sleep(3000); //замедлить на случай не получения бана

                    //парсинг статы
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(driver.PageSource);

                    //парсинг нужных узлов
                    string date = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'duelParticipant__startTime')]").InnerText.Trim().Split(' ')[0].Trim();
                    string time = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'duelParticipant__startTime')]").InnerText.Trim().Split(' ')[1].Trim();
                    string score = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'detailScore__wrapper')]").InnerText;
                    string status = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'detailScore__status')]").InnerText.ToLower();
                    string homeName = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'duelParticipant__home')]//a[contains(@class, 'participant__participantName')]").InnerText;
                    string awayName = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'duelParticipant__away')]//a[contains(@class, 'participant__participantName')]").InnerText;
                    var statsNodes = doc.DocumentNode.SelectNodes("//div[@class='stat__row']");

                    //собираем список команд в лиге
                    league.TeamNames.Add(homeName);
                    league.TeamNames.Add(awayName);

                    int homeScore = Convert.ToInt32(score.Split('-')[0].Trim());
                    int awayScore = Convert.ToInt32(score.Split('-')[1].Trim());

                    var homeStats = new Dictionary<string, List<StatBar>>
                    {
                        { "матч", new List<StatBar>() }
                    };
                    var awayStats = new Dictionary<string, List<StatBar>>
                    {
                        { "матч", new List<StatBar>() }
                    };
                    
                    foreach (var stat in statsNodes)
                    {
                        string title = stat.SelectSingleNode(".//div[@class = 'stat__categoryName']/text()").InnerText.Trim();
                        string homeValue = stat.SelectSingleNode(".//div[@class = 'stat__homeValue']").InnerText.Trim(new char[] { '%' });
                        string awayValue = stat.SelectSingleNode(".//div[@class = 'stat__awayValue']").InnerText.Trim(new char[] { '%' });

                        homeStats["матч"].Add(new StatBar(title, Convert.ToDouble(homeValue.Replace('.', ','))));
                        awayStats["матч"].Add(new StatBar(title, Convert.ToDouble(awayValue.Replace('.', ','))));

                        //собираем список статы в лиге
                        league.StatBarTitles.Add(title);
                    }

                    //GetPlayerStats(driver, match);

                    //Заполняем данные по домашней команде
                    games.Add(new TeamGame
                    {
                        LeagueTitle = match.LeagueTitle,
                        MatchID = match.ID,
                        Date = date,
                        Time = time,
                        Status = status,
                        URL = match.Url,
                        Owner = new Team(homeName),
                        Rival = new Team(awayName),
                        IsHome = true,
                        Score = homeScore,
                        Form = homeScore > awayScore ? WIN : (homeScore < awayScore ? LOSE : DRAW),
                        TeamStats = homeStats
                    });
                    //заполняем данные по гостевой команде
                    games.Add(new TeamGame
                    {
                        LeagueTitle = match.LeagueTitle,
                        MatchID = match.ID,
                        Date = date,
                        Time = time,
                        Status = status,
                        URL = match.Url,
                        Owner = new Team(awayName),
                        Rival = new Team(homeName),
                        IsHome = false,
                        Score = awayScore,
                        Form = awayScore > homeScore ? WIN : (awayScore < homeScore ? LOSE : DRAW),
                        TeamStats = awayStats
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR: {ex.Message}");
                }
            }

            league.Games = games;
        }

        //метод пишетсяв тестовом режиме... доработать
        //подумать куда собирать стату что бы не грамоздить
        internal (string Home, string Away) GetPlayerStats(IWebDriver driver, Match match)
        {
            List<string> urls = new List<string>()
            {
                $"https://www.flashscore.com.ua/match/{match.ID.Substring(4)}/#/match-summary/player-statistics/1",
                $"https://www.flashscore.com.ua/match/{match.ID.Substring(4)}/#/match-summary/player-statistics/2"
            };

            foreach(string url in urls)
            {
                var tmp = new Dictionary<string, string>();
                var head = new List<string>();
                var body = new List<string>();

                driver.Navigate().GoToUrl(url);
                //ожидаем подгрузки таблицы со статой
                new WebDriverWait(driver, TimeSpan.FromSeconds(10)).Until(ExpectedConditions.ElementExists(By.XPath("//div[contains(@class, 'section psc__section')]")));

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(driver.PageSource);
                var playerStatsNodes = doc.DocumentNode.SelectNodes("//div[contains(@class,'ui-table playerStatsTable')]");

                /* иерархия таблицы
                 * ui-table playerStatsTable
                 * 
                 *      ui-table__header
                 *              ui-table__headerCell
                 *              ui-table__headerCell
                 *              ui-table__headerCell
                 *              .......
                 *      
                 *      ui-table__body
                 *              ui-table__row playerStatsTable__row
                 *                      playerStatsTable__cell
                 *                      playerStatsTable__cell
                 *                      ...
                 *              ui-table__row playerStatsTable__row
                 *                      playerStatsTable__cell
                 *                      playerStatsTable__cell
                 *                      ...
                 *              ui-table__row playerStatsTable__row
                 *              .....
                 */

                foreach (var table in playerStatsNodes)
                {
                    var header = table.SelectNodes(".//div[contains(@class,'ui-table__headerCell')]");
                    foreach(var hCell in header)
                    {
                        head.Add(hCell.InnerText);
                    }

                    var bodys = table.SelectNodes(".//*[contains(@class,'playerStatsTable__cell')]");

                    foreach( var bCell in bodys)
                    {
                        body.Add(bCell.InnerText);
                    }
                }
            }

            return ("123", "321");
        }
    }
}
