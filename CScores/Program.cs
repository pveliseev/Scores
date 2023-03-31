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
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(7);

            /*
             * парсим список матчей
             * из списка формируем список команд
             * при парсе статистики записываем статистику в команду
             * массив статистики имеет атрибуты: домашняя?, соперник, ид матча
             * 
             */

            //создаем список лиг (название, url) для парсинга
            List<League> leagues = new List<League>()
            {
                new League(Sport.Baseball, "США МЛБ 2023 - Предсезонка", @"https://www.flashscore.com.ua/baseball/usa/mlb/results/")
            };
            //задаем парсер
            Parser parser = new FlashScoreParser();

            try
            {
                foreach (League league in leagues)
                {
                    parser.GetMatches(driver, league);
                    league.TeamsInit();

                    int count = league.Matches.Count;
                    for (int i = 0; i < 1; i++)
                    {
                        //parser.GetMatchStats(driver, league);
                    }
                }
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
