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
            League mlb = new League(Sport.Baseball);
            mlb.Pages.Add(new Page("MLB 2023", @"https://www.flashscore.com.ua/baseball/usa/mlb/results/"));
            //mlb.Pages.Add(new Page("MLB 2022", @"https://www.flashscore.com.ua/baseball/usa/mlb-2022/results/"));

            //League seriaA = new League(Sport.Football);
            //seriaA.Pages.Add(new Page("SeriaA 2022-2023", @"https://www.flashscore.com.ua/football/italy/serie-a/results/"));
            //seriaA.Pages.Add(new Page("SeriaA 2021-2022", @"https://www.flashscore.com.ua/football/italy/serie-a-2021-2022/results/"));

            //List<League> list = new List<League>();
            //list.Add(mlb);
            //list.Add(seriaA);

            try
            {
                Parser parser = new FlashScoreParser();
                parser.GetMatches(driver, mlb);
                parser.GetTeamGames(driver, mlb);
            }
            finally
            {


                //конец программы
                sw.Stop();
                Console.WriteLine($"Время выпонения скрипта: {sw.Elapsed}");
                Console.WriteLine("Нажмите любую клавишу для завершения...");
                //Console.ReadKey();
                driver.Quit();
            }
        }
    }
}
