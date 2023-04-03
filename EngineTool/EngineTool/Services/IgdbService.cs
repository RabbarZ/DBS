using EngineTool.Models;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace EngineTool.Services
{
    public class IgdbService
    {
        private readonly HttpClient http;

        public IgdbService()
        {
            this.http = new HttpClient();
        }

        public async Task<Game[]> GetGamesAsync(int count)
        {
            http.DefaultRequestHeaders.Add("Client-ID", "8lihv1kzozi9iiq0nqxjk5wsrjlf45");
            http.DefaultRequestHeaders.Add("Authorization", "Bearer blf6xjlxuvlxbqaq5vqdziobmf3931");

            var res = await http.PostAsync("https://api.igdb.com/v4/games", new StringContent("limit 100;fields name,game_engines.*,websites.*;where name = \"Hunt: Showdown\";"));
            var content = await res.Content.ReadFromJsonAsync<Game[]>();

            return content;
        }
    }
}
