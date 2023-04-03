using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
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

        public async Task GetCurrentPlayerCountAsync(string steamAppId)
        {
            var res = await http.GetAsync($"https://api.steampowered.com/ISteamUserStats/GetNumberOfCurrentPlayers/v1/?appid={steamAppId}");
            var content = await res.Content.ReadAsStringAsync();
        }

        public async Task GetRatingAsync(string steamAppId)
        {
            var res = await http.GetAsync($"https://store.steampowered.com/appreviews/{steamAppId}?json=1");
            var content = await res.Content.ReadAsStringAsync();
        }
    }
}
