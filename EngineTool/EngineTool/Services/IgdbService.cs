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
            http.DefaultRequestHeaders.Add("Client-ID", "0pibid6ji6736bjra5w03tuz2iieot");
            http.DefaultRequestHeaders.Add("Authorization", "Bearer w9u7gnhbtlpdxlkfznw67xrbs2cy6v");
        }

        public async Task<List<IgdbGame>> GetGamesAsync(int count)
        {
            List<IgdbGame> games = [];
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
