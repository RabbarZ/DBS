using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task GetGames(int count)
        {
            //const int maxCount = 100;
            //int iterations = count % maxCount;
            //count -= iterations * maxCount;
            //if (count > 0)
            //{
            //    iterations++;
            //}

            //for (int i = 0; i > iterations; i++)
            //{
            //    if (i == iterations)
            //    {

            //    }
            //}

            http.DefaultRequestHeaders.Add("Client-ID", "8lihv1kzozi9iiq0nqxjk5wsrjlf45");
            http.DefaultRequestHeaders.Add("Authorization", "Bearer blf6xjlxuvlxbqaq5vqdziobmf3931");

            var res = await http.PostAsync("https://api.igdb.com/v4/games", new StringContent("limit 100;fields name,game_engines.*,websites.*;where name = \"Hunt: Showdown\";"));
            var content = await res.Content.ReadAsStringAsync();
        }
    }
}
