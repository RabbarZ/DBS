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

        public async Task<SteamPlayerStatsResponse> GetCurrentPlayerCountAsync(int steamAppId)
        {
            var response = await http.GetFromJsonAsync<SteamPlayerStatsResponse>($"https://api.steampowered.com/ISteamUserStats/GetNumberOfCurrentPlayers/v1/?appid={steamAppId}");
            return response;
        }

        public async Task<SteamQuerySummary> GetRatingAsync(int steamAppId)
        {
            var response = await http.GetFromJsonAsync<SteamQuerySummary>($"https://store.steampowered.com/appreviews/{steamAppId}?json=1&num_per_page=0");
            return response;
        }
    }
}
