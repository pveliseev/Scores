using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace CScores
{
    internal class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Stopwatch sw = new Stopwatch();
            sw.Start();

            //автообновление экзешника драйвера
            new DriverManager().SetUpDriver(new ChromeConfig());

            //настройка и запуск драйвера хрома
            ChromeOptions options = new ChromeOptions();
            //стратегия загрузки страницы
            options.PageLoadStrategy = PageLoadStrategy.Eager;
            //options.AddArguments("headless", "disable-gpu");
            options.AddArgument("--disable-blink-features=AutomationControlled"); //скрывает что работает автодрайвер
            options.AddArgument("--start-maximized");
            //options.AddArgument("--disable-logging");
            //options.AddArgument("--log-level=3");
            //options.AcceptInsecureCertificates = true;

            IWebDriver driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            //входные данные для парсинга
            League league = new League(Sport.Baseball);
            //league.Pages.Add(new Page("Суперлига 2022", @"https://www.flashscore.com.ua/football/china/super-league-2022/results/"));
            //league.Pages.Add(new Page("Суперлига 2021", @"https://www.flashscore.com.ua/football/china/super-league-2021/results/"));
            //league.Pages.Add(new Page("Суперлига 2020", @"https://www.flashscore.com.ua/football/china/super-league-2020/results/"));
            //league.Pages.Add(new Page("Суперлига 2019", @"https://www.flashscore.com.ua/football/china/super-league-2019/results/"));
            //league.Pages.Add(new Page("Суперлига 2018", @"https://www.flashscore.com.ua/football/china/super-league-2018/results/"));
            //league.Pages.Add(new Page("МЛБ 2023", @"https://www.flashscore.com.ua/baseball/usa/mlb/results/"));
            //league.Pages.Add(new Page("Премьер-лига 2022/2023", @"https://www.flashscore.com.ua/football/russia/premier-league-2022-2023/results/"));

            try
            {
                //Parser parser = new FlashScoreFootballParser();
                //parser.GetMatches(driver, league);
                //parser.GetTeamGames(driver, league);

                Parser parser = new SportLigaTableTennis();
                ((SportLigaTableTennis)parser).GetPages(driver,league, new DateTime(2023,7,10), new DateTime(2023, 7, 13));
                ((SportLigaTableTennis)parser).GetIndividualGame(driver, league);
            }
            finally
            {
                Print.TableTennis(league);
                //конец программы
                sw.Stop();
                Console.WriteLine($"Время выпонения скрипта: {sw.Elapsed}");
                Console.WriteLine("Нажмите любую клавишу для завершения...");
                driver.Quit();
                Console.ReadKey();
            }
        }
    }
}
