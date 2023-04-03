using EngineTool.Models;
using System.Net.Http.Json;

namespace EngineTool.Services
{
    public class SteamService
    {
        private readonly HttpClient http;

        public SteamService()
        {
            this.http = new HttpClient();
        }

        public async Task<int> GetCurrentPlayerCountAsync(string steamAppId)
        {
            var res = await http.GetFromJsonAsync<SteamPlayerStatsResponse>($"https://api.steampowered.com/ISteamUserStats/GetNumberOfCurrentPlayers/v1/?appid={steamAppId}");
            if (res.PlayerStats.Success != 1)
            {
                throw new Exception("Failure due to unknown error.");
            }

            var content = res.PlayerStats.PlayerCount;
            return content;
        }

        public async Task<IgdbRating> GetRatingAsync(string steamAppId)
        {
            var res = await http.GetFromJsonAsync<SteamQuerySummary>($"https://store.steampowered.com/appreviews/{steamAppId}?json=1&num_per_page=0");
            if (res.Success != 1)
            {
                throw new Exception("Failure due to unknown error.");
            }

            var content = res.Rating;           
            return content;
        }
    }
}
