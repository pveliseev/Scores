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
}