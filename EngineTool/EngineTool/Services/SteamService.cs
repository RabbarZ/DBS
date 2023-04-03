using EngineTool.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
            var content = res.PlayerStats.PlayerCount;

            return content;
        }

        public async Task<IgdbRating> GetRatingAsync(string steamAppId)
        {
            var res = await http.GetFromJsonAsync<SteamQuerySummary>($"https://store.steampowered.com/appreviews/{steamAppId}?json=1");
            var content = res.Rating;           

            return content;
        }
    }
}
