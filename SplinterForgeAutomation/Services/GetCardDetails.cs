using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SplinterForgeAutomation.Models;

namespace SplinterForgeAutomation.Services
{
    public class GetCardDetails
    {
        public async Task<CardDetailsResponse> GetPlayerCardDetails(Player player)
        {
            try
            {
                string user = player.Username;
                string token = player.Token;


                using (var httpClient = new HttpClient())
                {
                    var cardDetailsPayload = new
                    {
                        user
                    };

                    var cardDetailsRequest = new HttpRequestMessage
                    {
                        RequestUri = new Uri("https://api2.splinterlands.com/cards/collection/" + user),
                        Method = HttpMethod.Get,
                        Content = new StringContent(JsonConvert.SerializeObject(cardDetailsPayload), System.Text.Encoding.UTF8, "application/json")
                    };

                    HttpResponseMessage cardDetailsResponse = await httpClient.SendAsync(cardDetailsRequest);

                    if (cardDetailsResponse.IsSuccessStatusCode)
                    {
                        string CardDetailsResponse = await cardDetailsResponse.Content.ReadAsStringAsync();
                        Console.WriteLine($"Card received for player {player.Username}!");

                        // Deserialize JSON into CardDetailsResponse object
                        var cardDetails = JsonConvert.DeserializeObject<CardDetailsResponse>(CardDetailsResponse);
                        return cardDetails;
                    }
                    else
                    {
                        Console.WriteLine($"Failed to find cards for player for {player.Username}. Status code: {cardDetailsResponse.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during daily claim: {ex.Message}");
            }

            return null;
        }
    }


}
