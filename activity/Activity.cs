using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace activity
{
    internal class Activity
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        IntPtr handle;

        public void ConsoleOperations()
        {
            handle = GetConsoleWindow();

            string[] tokens = File.ReadAllLines("tokens.txt");

            Console.WriteLine("Loaded " + tokens.Length + " accounts");
            Console.WriteLine("[1] Launch Headless + Hide Console");
            Console.WriteLine("[2] Launch Headless");
            Console.WriteLine("[3] Launch Specific Token");
            Console.WriteLine("[4+] Launch All Normally");

            ConsoleKeyInfo cki = new ConsoleKeyInfo();
            cki = Console.ReadKey();

            bool headless = false;
            bool hideConsole = false;
            if (cki.Key == ConsoleKey.D1)
            {
                headless = true;
                hideConsole = true;
            }
            else if (cki.Key == ConsoleKey.D2)
            {
                headless = true;
            }
            else if (cki.Key == ConsoleKey.D3)
            {
                Console.WriteLine("What token:");
                for (int i = 0; i < tokens.Length; i++)
                {
                    Console.WriteLine($"[{i}] {tokens[i]}");
                }

                int num = Convert.ToInt32(Console.ReadLine());

                if (num >= 0 && num < tokens.Length)
                {
                    tokens = tokens.Where((t, i) => i == num).ToArray();
                }
            }

            Console.Clear();

            if (hideConsole)
            {
                ShowWindow(handle, SW_HIDE);
            }
            login(tokens, headless);
        }

        public void addToTaskbar(IntPtr handle)
        {
            NotifyIcon trayIcon = new NotifyIcon
            {
                Icon = new Icon("C:\\Users\\ZipTop\\source\\repos\\activity\\activity\\icon.ico"),
                Text = "Discord Activity",
                Visible = true
            };

            ContextMenu contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add("Show", (sender, e) => ShowWindow(handle, SW_SHOW));

            trayIcon.ContextMenu = contextMenu;
        }

        void login(string[] tokens, bool isHeadless)
        {
            List<IWebDriver> drivers = new List<IWebDriver>();
            foreach (var token in tokens)
            {
                var chromeOptions = new ChromeOptions();
                if (isHeadless) chromeOptions.AddArguments("headless");
                chromeOptions.AddArguments("log-level=3", "--silent", "--disable-logging");

                var service = ChromeDriverService.CreateDefaultService();
                service.SuppressInitialDiagnosticInformation = true;
                service.HideCommandPromptWindow = true;
                service.EnableVerboseLogging = false;

                var driver = new ChromeDriver(service, chromeOptions);
                drivers.Add(driver);

                driver.Navigate().GoToUrl("https://discord.com/login");

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(wd => ((IJavaScriptExecutor)wd).ExecuteScript("return document.readyState").ToString() == "complete");

                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript($"document.body.appendChild(document.createElement `iframe`).contentWindow.localStorage.token = `\"{token}\"`");

                driver.Navigate().Refresh();
            }

            Console.WriteLine("Script execution completed. Press any key to close all browsers.");
            Console.ReadKey();

            foreach (var driver in drivers)
            {
                driver.Quit();
            }

            Environment.Exit(0);
        }
    }
}
