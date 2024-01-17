using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SplinterForgeAutomation.Models;
using SplinterForgeAutomation.Services;


namespace SplinterForgeAuto
{
    class Program
    {
        private static bool exitLoop = false;

        static async Task Main(string[] args)
        {
            try
            {
                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                //string driverPath = Path.Combine(currentDirectory, "chromedriver.exe");
                //string crxFilePath = Path.Combine(currentDirectory, "hivekeychain.crx");
                //string extensionId = "jcacnejopjdphbnjgfaaobbfafkihpep";

               // await PerformActions(driverPath, crxFilePath, extensionId);

                var actionService = new ActionService();
                await actionService.PerformActions();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


    }
}
