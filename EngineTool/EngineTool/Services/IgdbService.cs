using EngineTool.Models;
using System.Net.Http.Json;

namespace EngineTool.Services
{
    public class IgdbService
    {
        private const int MaxCount = 500;
        private readonly HttpClient http;

        public IgdbService()
        {
            this.http = new HttpClient();
        }

        public async Task<List<Game>> GetGamesAsync(int count)
        {
            http.DefaultRequestHeaders.Add("Client-ID", "8lihv1kzozi9iiq0nqxjk5wsrjlf45");
            http.DefaultRequestHeaders.Add("Authorization", "Bearer blf6xjlxuvlxbqaq5vqdziobmf3931");

            List<Game> games = new List<Game>();
            for (int i = 0; i < count / MaxCount; i++)
            {
                var offset = i * MaxCount;
                var res = await http.PostAsync("https://api.igdb.com/v4/games", new StringContent($"offset {offset}; limit {MaxCount};fields name,game_engines.name,game_engines.id,websites.category,websites.url;where websites.category = 13 & game_engines != null & category = (0, 8, 9);"));
                var content = await res.Content.ReadFromJsonAsync<Game[]>();
                games.AddRange(content);
            }

            return games;
        }
    }
}
