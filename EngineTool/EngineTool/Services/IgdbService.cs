using EngineTool.Config;
using EngineTool.Interfaces;
using EngineTool.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace EngineTool.Services
{
    public class IgdbService(IOptions<IgdbApiSettings> igdbApiSettings, HttpClient httpClient) : IIgdbService
    {
        private const int MaxCount = 500;
        private readonly HttpClient http = httpClient;
        private readonly IgdbApiSettings igdbApiSettings = igdbApiSettings.Value;

        public async IAsyncEnumerable<IgdbGame> GetGamesAsync(int count)
        {
            int limit = MaxCount;
            if (count < MaxCount)
            {
                limit = count;
            }

            for (int i = 0; i < count / limit; i++)
            {
                var offset = i * limit;

                var res = await http.PostAsync(igdbApiSettings.Url, new StringContent($"offset {offset}; limit {limit};fields name,game_engines.name,game_engines.id,websites.category,websites.url;where websites.category = 13 & game_engines != null & category = (0, 8, 9);"));
                var games = await res.Content.ReadFromJsonAsync<IgdbGame[]>();
                if (games != null)
                {
                    foreach (var game in games)
                    {
                        yield return game;
                    }
                }
            }
        }

        public int? GetSteamId(IgdbGame game)
        {
            string? steamUrl = game.Websites.SingleOrDefault(w => w.Category == 13)?.Url;
            if (steamUrl == null)
            {
                return null;
            }

            if (int.TryParse(steamUrl.Split('/')[4], out int steamId))
            {
                return steamId;
            }

            return null;
        }
    }
}
