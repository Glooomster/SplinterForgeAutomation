using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SplinterForgeAutomation.Models;

namespace SplinterForgeAutomation.Services
{
    public class HeroDetailsService
    {
        public async Task<HeroDetailsResponse[]> GetHeroDetails(Player player)
        {
            try
            {
                string user = player.Username;
                string token = player.Token;

                using (var httpClient = new HttpClient())
                {
                    var heroDetailsRequest = new HttpRequestMessage
                    {
                        RequestUri = new Uri($"https://splinterforge.io/hero/getHeroes?user={user}&token={token}"),
                        Method = HttpMethod.Get,
                    };

                    HttpResponseMessage heroDetailsResponse = await httpClient.SendAsync(heroDetailsRequest);

                    if (heroDetailsResponse.IsSuccessStatusCode)
                    {
                        string heroDetailsResponseString = await heroDetailsResponse.Content.ReadAsStringAsync();
                        Console.WriteLine($"Hero details received for player {player.Username}!");

                        // Deserialize JSON into HeroDetailsResponse array
                        var heroDetails = JsonConvert.DeserializeObject<HeroDetailsResponse[]>(heroDetailsResponseString);
                        return heroDetails;
                    }
                    else
                    {
                        Console.WriteLine($"Failed to get hero details for player {player.Username}. Status code: {heroDetailsResponse.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during hero details retrieval: {ex.Message}");
            }

            return null;
        }
    }
}
