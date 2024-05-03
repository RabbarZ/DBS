using EngineTool.Interfaces;
using EngineTool.Models;
using System.Net.Http.Json;

namespace EngineTool.Services
{
    public class SteamService : ISteamService
    {
        private readonly HttpClient http;

        public SteamService(HttpClient httpClient)
        {
            this.http = httpClient;
        }

        public async Task<int?> GetCurrentPlayerCountAsync(int steamAppId)
        {
            var response = await http.GetFromJsonAsync<SteamPlayerStatsResponse>($"https://api.steampowered.com/ISteamUserStats/GetNumberOfCurrentPlayers/v1/?appid={steamAppId}");
            return response?.PlayerStats.PlayerCount;
        }

        public async Task<SteamRating?> GetRatingAsync(int steamAppId)
        {
            var response = await http.GetFromJsonAsync<SteamQuerySummary>($"https://store.steampowered.com/appreviews/{steamAppId}?json=1&num_per_page=0");
            return response?.Rating;
        }
    }
}
