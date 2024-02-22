using Newtonsoft.Json;
using SplinterForgeAutomation.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SplinterForgeAutomation.Services
{
    public class ActionService
    {
        private int intervalMinutes = 1;
        private static int defaultWaitMinutes = 60;
        private static int userWaitMinutes = 0;
        private Dictionary<string, DateTime> lastBossFightTimes = new Dictionary<string, DateTime>();

        public async Task PerformActions()
        {
            try
            {
                int defaultInterval = 1;
                Console.WriteLine("Enter the interval between actions in minutes (default is 1 minute): ");

                var userInputTask = Task.Run(() => Console.ReadLine());

                if (await Task.WhenAny(userInputTask, Task.Delay(TimeSpan.FromSeconds(10))) == userInputTask)
                {
                    if (int.TryParse(userInputTask.Result, out int userInterval) && userInterval > 0)
                    {
                        intervalMinutes = userInterval;
                    }
                }
                else
                {
                    Console.WriteLine($"No input received within 10 seconds. Using the default interval of {defaultInterval} minute(s).");
                    intervalMinutes = defaultInterval;
                }



                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string jsonFilePath = Path.Combine(currentDirectory, "config.json");

                string jsonContent = File.ReadAllText(jsonFilePath);

                var users = JsonConvert.DeserializeObject<List<Player>>(jsonContent);
                var bossFight = new BossFight();
                var dailyClaim = new DailyClaim();
                var cardDetails = new GetCardDetails();
                var heroCardDetails = new HeroDetailsService();

                int minWait = intervalMinutes;
                Console.WriteLine("Press ESC to stop the loop...\n");

                while (true)
                {

                    Console.WriteLine($"\nPerforming actions at: {DateTime.Now}");

                    foreach (var player in users)
                    {
                        Console.WriteLine($"\nStarting requests for {player.Username}!");

                        // Get the last boss fight time for the user
                        DateTime lastBossFightTime = lastBossFightTimes.ContainsKey(player.Username)
                            ? lastBossFightTimes[player.Username]
                            : DateTime.MinValue;

                        // Check if enough time has passed since the last boss fight
                        bool performBossFight = DateTime.Now - lastBossFightTime >= TimeSpan.FromMinutes(60);

                        bool bossFightResult = true;

                        if (performBossFight)
                        {
                            // Get card details and store in variable
                            var playerCardDetails = await cardDetails.GetPlayerCardDetails(player);

                            var teamIDs = player.Team
                                .Where(teamId => !teamId.EndsWith(" hero"))
                                .Select(teamId => teamId.EndsWith(" hero") ? teamId.Substring(0, teamId.Length - 5) : teamId)
                                .ToArray();

                            var matchingCardIDs = teamIDs
                                .Select(teamId => playerCardDetails.Cards
                                .Where(card => card.Uid.Contains($"-{teamId}-") &&
                                (card.Delegated_to == player.Username || card.Delegated_to == null))
                                    .OrderByDescending(card => card.Xp)
                                    .FirstOrDefault()?.Uid.ToString())
                                .Where(cardId => !string.IsNullOrEmpty(cardId))
                                .ToArray();

                            var heroDetails = await heroCardDetails.GetHeroDetails(player);

                            if (heroDetails != null)
                            {
                                var playerHeroType = player.Hero.ToLower(); // Ensure consistency in casing

                                var hero = heroDetails.FirstOrDefault(h => h.Type.ToLower() == playerHeroType);
                                if (hero != null && !string.IsNullOrEmpty(hero.Id))
                                {
                                    matchingCardIDs = matchingCardIDs.Append(hero.Id + " hero").ToArray();
                                }
                                else
                                {
                                    Console.WriteLine($"Hero type '{player.Hero}' not found in hero details.");
                                }
                            }

                            player.FightTeam = matchingCardIDs;

                            // Perform boss fight using a separate BossFight class
                            bossFightResult = await bossFight.PerformBossFight(player);


                            if (bossFightResult == false)
                            {
                                // Update the last boss fight time for the user
                                lastBossFightTimes[player.Username] = DateTime.Now;
                            }

                            await dailyClaim.ClaimDailyReward(player);


                        }
                        else
                        {
                            Console.WriteLine($"'{player.Username}' is taking break for x amount of time.");
                        }


                    }

                    Console.WriteLine($"Waiting for {minWait} minutes...");
                    await Task.Delay(TimeSpan.FromMinutes(minWait));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }g
    }
}
