using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SplinterForgeAutomation.Models;

namespace SplinterForgeAutomation.Services
{
    public class BossFight
    {
        public async Task PerformBossFight(Player player)
        {
            try
            {
                string username = player.Username;
                string token = player.Token;
                string type = player.Type;
                string[] team = player.FightTeam;
                int gear = player.Gear;
                string source = player.Source;
                string user = player.Username;



                using (var httpClient = new HttpClient())
                {
                    var getRequest = new HttpRequestMessage
                    {
                        RequestUri = new Uri($"https://splinterforge.io/users/requestEncodedMessage?user={username}&source=fight%20boss&type=Posting"),
                        Method = HttpMethod.Get
                    };

                    HttpResponseMessage getResponse = await httpClient.SendAsync(getRequest);

                    if (getResponse.IsSuccessStatusCode)
                    {
                        string encodedMessage = await getResponse.Content.ReadAsStringAsync();

                        var payload = new
                        {
                            username,
                            token,
                            type,
                            team,
                            memo = "gtg",
                            gear,
                            source,
                            encoded_message = encodedMessage
                        };

                        var postRequest = new HttpRequestMessage
                        {
                            RequestUri = new Uri("https://splinterforge.io/boss/fight_boss"),
                            Method = HttpMethod.Post,
                            Content = new StringContent(JsonConvert.SerializeObject(payload), System.Text.Encoding.UTF8, "application/json")
                        };

                        HttpResponseMessage postResponse = await httpClient.SendAsync(postRequest);

                        if (postResponse.IsSuccessStatusCode)
                        {
                            string apiResponse = await postResponse.Content.ReadAsStringAsync();
                            Console.WriteLine("POST request successful!");
                            Console.WriteLine("Response:");

                            dynamic jsonResponse = JsonConvert.DeserializeObject(apiResponse);


                            if (jsonResponse.message != null && jsonResponse.message == "not enough mana!")
                            {
                                Console.WriteLine("Not enough mana!");
                            }
                            else if (jsonResponse.actions != null && jsonResponse.totalDmg != null && jsonResponse.points != null && jsonResponse.rewards != null)
                            {
                                Console.WriteLine($"Actions: {jsonResponse.actions}");
                                Console.WriteLine($"Total Damage: {jsonResponse.totalDmg}");
                                Console.WriteLine($"Points: {jsonResponse.points}");
                                Console.WriteLine("Rewards:");
                                foreach (var reward in jsonResponse.rewards)
                                {
                                    Console.WriteLine($"Type: {reward.type}, Name: {reward.name}, Quantity: {reward.qty}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Unknown response format.");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Failed to make POST request. Status code: {postResponse.StatusCode}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Failed to get the encoded message. Status code: {getResponse.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during boss fight: {ex.Message}");
            }
        }
    }
}
