using Azure.Core.Pipeline;
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
        private readonly HttpClient httpClient;
        private readonly IgdbApiSettings igdbApiSettings;

        public IgdbService(IOptions<IgdbApiSettings> igdbApiSettings)
        {
            this.igdbApiSettings = igdbApiSettings.Value;
            this.httpClient = new HttpClient();
            this.httpClient.DefaultRequestHeaders.Add("Client-ID", this.igdbApiSettings.ClientId);
            this.httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.igdbApiSettings.BearerToken}");
        }

        public async Task<List<IgdbGame>> GetGamesAsync(int count)
        {
            List<IgdbGame> games = [];
            for (int i = 0; i < count / MaxCount; i++)
            {
                var offset = i * MaxCount;
                var res = await httpClient.PostAsync(igdbApiSettings.Url, new StringContent($"offset {offset}; limit {MaxCount};fields name,game_engines.name,game_engines.id,websites.category,websites.url;where websites.category = 13 & game_engines != null & category = (0, 8, 9);"));
                var content = await res.Content.ReadFromJsonAsync<IgdbGame[]>();
                games.AddRange(content);
            }

            return games;
        }
    }
}
