using Azure.Core.Pipeline;
using EngineTool.Config;
using EngineTool.Interfaces;
using EngineTool.Models;
using System.Net.Http.Json;

namespace EngineTool.Services
{
    public class IgdbService : IIgdbService
    {
        private const int MaxCount = 500;
        private readonly HttpClient httpClient;
        private readonly AppSettings appSettings;

        public IgdbService(AppSettings appSettings, HttpClient httpClient)
        {
            this.appSettings = appSettings;
            this.httpClient = httpClient;
            this.httpClient.DefaultRequestHeaders.Add("Client-ID", this.appSettings.ClientId);
            this.httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.appSettings.BearerToken}");
        }

        public async Task<List<IgdbGame>> GetGamesAsync(int count)
        {
            List<IgdbGame> games = [];
            for (int i = 0; i < count / MaxCount; i++)
            {
                var offset = i * MaxCount;
                var res = await httpClient.PostAsync(appSettings.IGDBAPIURL, new StringContent($"offset {offset}; limit {MaxCount};fields name,game_engines.name,game_engines.id,websites.category,websites.url;where websites.category = 13 & game_engines != null & category = (0, 8, 9);"));
                var content = await res.Content.ReadFromJsonAsync<IgdbGame[]>();
                games.AddRange(content);
            }

            return games;
        }
    }
}
