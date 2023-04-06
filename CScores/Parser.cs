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
        public abstract void GetMatches(IWebDriver driver, League league);
        public abstract void GetGames(IWebDriver driver, League league);
    }

    internal class XScoresParser : Parser
    {
        public override void GetMatches(IWebDriver driver, League league)
        {

        }
        public override void GetGames(IWebDriver driver, League league)
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
        public override void GetGames(IWebDriver driver, League league)
        {
            //инициализация коллекций которые будут здесь заполнены
            league.TeamNames = new HashSet<string>();
            league.Games = new List<Game>(); //определять выбор типа игры
            league.StatBarTitles = new HashSet<string>();

            int cnt = league.Matches.Count;
            for (int i = 0; i < 3; i++)
            {

            }
        }
    }
}
