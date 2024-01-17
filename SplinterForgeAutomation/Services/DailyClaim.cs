using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SplinterForgeAutomation.Models;

namespace SplinterForgeAutomation.Services
{
    public class DailyClaim
    {
        public async Task ClaimDailyReward(Player player)
        {
            try
            {
                string user = player.Username;
                string token = player.Token;

                using (var httpClient = new HttpClient())
                {
                    var claimDailyPayload = new
                    {
                        user,
                        token
                    };

                    var claimDailyRequest = new HttpRequestMessage
                    {
                        RequestUri = new Uri("https://splinterforge.io/users/claimDaily"),
                        Method = HttpMethod.Post,
                        Content = new StringContent(JsonConvert.SerializeObject(claimDailyPayload), System.Text.Encoding.UTF8, "application/json")
                    };

                    HttpResponseMessage claimDailyResponse = await httpClient.SendAsync(claimDailyRequest);

                    if (claimDailyResponse.IsSuccessStatusCode)
                    {
                        string claimResponse = await claimDailyResponse.Content.ReadAsStringAsync();
                        Console.WriteLine($"Daily claim successful for {player.Username}!");
                        Console.WriteLine("Claim Response:");
                        Console.WriteLine(claimResponse);
                    }
                    else
                    {
                        Console.WriteLine($"Failed to claim daily for {player.Username}. Status code: {claimDailyResponse.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during daily claim: {ex.Message}");
            }
        }
    }
}
