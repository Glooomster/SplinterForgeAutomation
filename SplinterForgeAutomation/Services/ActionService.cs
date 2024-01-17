using Newtonsoft.Json;
using SplinterForgeAutomation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplinterForgeAutomation.Services
{
    public class ActionService
    {
        //private static bool exitLoop = false;

        public async Task PerformActions()
        {
            try
            {
                bool exitLoop = false;

                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string jsonFilePath = Path.Combine(currentDirectory, "config.json");

                string jsonContent = File.ReadAllText(jsonFilePath);

                var users = JsonConvert.DeserializeObject<List<Player>>(jsonContent);
                var bossFight = new BossFight();
                var dailyClaim = new DailyClaim();
                var cardDetails = new GetCardDetails();
                var heroCardDetails = new HeroDetailsService();


                int minWait = 1;
                Console.WriteLine("Press ESC to stop the loop...");

                while (true)
                {
                    if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
                    {
                        exitLoop = true;
                        break;
                    }

                    Console.WriteLine($"Performing actions at: {DateTime.Now}");

                    foreach (var player in users)
                    {
                        Console.WriteLine($"Starting requests for {player.Username}!");

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
                            if (hero != null)
                            {

                                if (!string.IsNullOrEmpty(hero.Id))
                                {
                                    matchingCardIDs = matchingCardIDs.Append(hero.Id + " hero").ToArray();
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Hero type '{player.Hero}' not found in hero details.");
                            }
                        }


                        player.FightTeam = matchingCardIDs;




                        // Perform boss fight using a separate BossFight class
                        await bossFight.PerformBossFight(player);


                        await dailyClaim.ClaimDailyReward(player);
                        }

                        Console.WriteLine($"Waiting for {minWait} minutes...");
                        await Task.Delay(TimeSpan.FromMinutes(minWait));
                    }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
