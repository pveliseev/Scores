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
            options.PageLoadStrategy = PageLoadStrategy.Normal;
            options.AddArguments("headless", "disable-gpu");
            options.AddArgument("--disable-blink-features=AutomationControlled"); //скрывает что работает автодрайвер
            //options.AddArgument("--start-maximized");
            //options.AddArgument("--disable-logging");
            //options.AddArgument("--log-level=3");
            //options.AcceptInsecureCertificates = true;

            IWebDriver driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            //входные данные для парсинга
            League league = new League(Sport.Football);
            league.Pages.Add(new Page("Олимп-ФНЛ 2021/2022", @"https://www.flashscore.com.ua/football/russia/fnl-2021-2022/results/"));
            league.Pages.Add(new Page("Олимп-ФНЛ 2020/2021", @"https://www.flashscore.com.ua/football/russia/fnl-2020-2021/results/"));
            league.Pages.Add(new Page("ФНЛ 2019/2020", @"https://www.flashscore.com.ua/football/russia/fnl-2019-2020/results/"));
            league.Pages.Add(new Page("ФНЛ 2018/2019", @"https://www.flashscore.com.ua/football/russia/fnl-2018-2019/results/"));
            league.Pages.Add(new Page("FNL 2017/2018", @"https://www.flashscore.com.ua/football/russia/fnl-2017-2018/results/"));

            try
            {
                Parser parser = new FlashScoreFootballParser();
                parser.GetMatches(driver, league);
                parser.GetTeamGames(driver, league);
            }
            finally
            {
                Print.Football(league);
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
