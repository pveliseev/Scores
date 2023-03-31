using HtmlAgilityPack;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CScores
{
    internal abstract class Parser
    {
        public abstract void GetMatches(IWebDriver driver, League league);
        public abstract void GetMatchStats(IWebDriver driver, League league);
    }

    internal class XScoresParser : Parser
    {
        public override void GetMatches(IWebDriver driver, League league)
        {

        }
        public override void GetMatchStats(IWebDriver driver, League league)
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
        public override void GetMatchStats(IWebDriver driver, League league)
        {

        }
    }
}
