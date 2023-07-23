using HtmlAgilityPack;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CScores
{
    internal class SportLigaTableTennis : Parser
    {
        public void GetPages(IWebDriver driver, League league, DateTime startDate, DateTime endDate)
        {
            DateTime tmp = startDate;
            do
            {
                //составить url из даты по формату сайта
                string toursUrl = string.Format($"https://tt.sport-liga.pro/tours/?year={tmp.Year}&month={tmp.Month.ToString().PadLeft(2, '0')}&day={tmp.Day}");

                //по сгенерированому адресу взять страницы: название турнира, url
                driver.Navigate().GoToUrl(toursUrl);
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(driver.PageSource);
                HtmlNodeCollection pagesNodes = doc.DocumentNode.SelectNodes("//table[@class='table']//td[@class='tournament-name']//a");
                foreach (var page in pagesNodes)
                {
                    //TODO костыль: к заголовку прикрепляем дату что бы при парсинге игр её использовать
                    string title = page.InnerText + ";" + tmp.ToString("d");

                    string url = "https://tt.sport-liga.pro/" + page.GetAttributeValue("href", "");

                    league.Pages.Add(new Page(title, url));
                }

                tmp = tmp.AddDays(1);
            } while (tmp <= endDate);
        }

        public void GetIndividualGame(IWebDriver driver, League league)
        {
            var games = new List<IndividualGame>();
            league.PlayerStatBarTitles = new HashSet<string>() { "1set", "2set", "3set", "4set", "5set" };

            //foreach (var page in league.Pages)
            {
                Page page = league.Pages[0]; // для теста одну таблицу
                driver.Navigate().GoToUrl(page.Url);

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(driver.PageSource);
                HtmlNodeCollection gamesNodes = doc.DocumentNode.SelectNodes("//table[@class='games_list']/tbody/tr");

                string type = string.Empty;

                foreach (var game in gamesNodes)
                {
                    //если в строке указана тип игры
                    if (game.SelectSingleNode(".//td[@class='center']") != null)
                    {
                        type = game.SelectSingleNode(".//td[@class='center']").InnerText;
                        continue;
                    }
                    //если в строке статистика игры, то парсим её
                    if (game.SelectSingleNode(".//td[@class='score']") != null)
                    {
                        //данные игры
                        string date = page.Title.Split(';')[1];
                        string time = game.SelectSingleNode("./td[1]/a").InnerText;
                        string gameUrl = "https://tt.sport-liga.pro/" + game.SelectSingleNode("./td[1]/a").GetAttributeValue("href", "");

                        string[] scores = game.SelectSingleNode("./td[4]/table//a").InnerText.Split(':');
                        int ownerScore = Convert.ToInt32(scores[0].Trim());
                        int rivalScore = Convert.ToInt32(scores[1].Trim());

                        //данные игрока слева
                        string ownerName = game.SelectSingleNode("./td[2]/a").InnerText;
                        string ownerID = game.SelectSingleNode("./td[2]/a").GetAttributeValue("href", "").Split('/')[1];
                        string ownerRating = game.SelectSingleNode("./td[3]/b").InnerText;

                        //данные игрока справа
                        string rivalName = game.SelectSingleNode("./td[6]/a").InnerText;
                        string rivalID = game.SelectSingleNode("./td[6]/a").GetAttributeValue("href", "").Split('/')[1];
                        string rivalRating = game.SelectSingleNode("./td[5]/b").InnerText;

                        Player owner = new Player { Name = ownerName, ID = ownerID, Rating = ownerRating };
                        Player rival = new Player { Name = rivalName, ID = rivalID, Rating = rivalRating };

                        //спарсили очки по сетам
                        var setsScore = game.SelectSingleNode("./td[4]/small").InnerText.Trim('(', ')').Split(' ');
                        for (int i = 0; i < setsScore.Length; i++)
                        {
                            owner.PlayerStats.Add(new StatBar(league.PlayerStatBarTitles.ElementAt(i), Convert.ToDouble(setsScore[i].Split('-')[0])));
                            rival.PlayerStats.Add(new StatBar(league.PlayerStatBarTitles.ElementAt(i), Convert.ToDouble(setsScore[i].Split('-')[1])));
                        }

                        //заполняем данные по игроку слева
                        games.Add(new IndividualGame
                        {
                            LeagueTitle = page.Title.Split(';')[0],
                            Date = date,
                            Time = time,
                            Status = type,
                            URL = gameUrl,
                            Owner = owner,
                            Rival = rival,
                            Score = ownerScore,
                            Form = ownerScore > rivalScore ? WIN : (ownerScore < rivalScore ? LOSE : DRAW)
                        });

                        //заполняем данные по игроку справа
                        games.Add(new IndividualGame
                        {
                            LeagueTitle = page.Title.Split(';')[0],
                            Date = date,
                            Time = time,
                            Status = type,
                            URL = gameUrl,
                            Owner = rival,
                            Rival = owner,
                            Score = rivalScore,
                            Form = rivalScore > ownerScore ? WIN : (rivalScore < ownerScore ? LOSE : DRAW)
                        });
                    }
                }
            }
            //запись игр в игры лиги
            league.Games = games;
        }

        public override void GetMatches(IWebDriver driver, League league)
        {
            throw new NotImplementedException();
        }

        public override void GetTeamGames(IWebDriver driver, League league)
        {
            throw new NotImplementedException();
        }
    }
}
