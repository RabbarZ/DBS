using EngineTool.Config;
using EngineTool.Interfaces;
using EngineTool.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace EngineTool.Services
{
    public class IgdbService : IIgdbService
    {
        private const int MaxCount = 500;
        private readonly HttpClient http;
        private readonly IgdbApiSettings igdbApiSettings;

        public IgdbService(IOptions<IgdbApiSettings> igdbApiSettings, HttpClient httpClient)
        {
            this.igdbApiSettings = igdbApiSettings.Value;
            this.http = httpClient;
        }

        public async Task<List<IgdbGame>> GetGamesAsync(int count)
        {
            List<IgdbGame> games = [];
            for (int i = 0; i < count / MaxCount; i++)
            {
                var offset = i * MaxCount;
                
                var res = await http.PostAsync(igdbApiSettings.Url, new StringContent($"offset {offset}; limit {MaxCount};fields name,game_engines.name,game_engines.id,websites.category,websites.url;where websites.category = 13 & game_engines != null & category = (0, 8, 9);"));
                var content = await res.Content.ReadFromJsonAsync<IgdbGame[]>();
                if (content != null)
                {
                    games.AddRange(content);
                }
            }

            return games;
        }
    }
}
