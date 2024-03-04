using EngineTool.Config;
using EngineTool.Models;
using System.Net.Http.Json;

namespace EngineTool.Services
{
    public class IgdbService
    {
        private const int MaxCount = 500;
        private readonly HttpClient http;
        private readonly AppSettings appSettings;

        public IgdbService(AppSettings appSettings)
        {
            this.appSettings = appSettings;
            this.http = new HttpClient();
            http.DefaultRequestHeaders.Add("Client-ID", "8lihv1kzozi9iiq0nqxjk5wsrjlf45");
            http.DefaultRequestHeaders.Add("Authorization", "Bearer kwom96yod6vvmzaf6v6d3t9gctsh9u");
        }

        public async Task<List<IgdbGame>> GetGamesAsync(int count)
        {
            List<IgdbGame> games = new();
            for (int i = 0; i < count / MaxCount; i++)
            {
                var offset = i * MaxCount;
                var res = await http.PostAsync(appSettings.IGDBAPIURL, new StringContent($"offset {offset}; limit {MaxCount};fields name,game_engines.name,game_engines.id,websites.category,websites.url;where websites.category = 13 & game_engines != null & category = (0, 8, 9);"));
                var content = await res.Content.ReadFromJsonAsync<IgdbGame[]>();
                games.AddRange(content);
            }

            return games;
        }
    }
}
