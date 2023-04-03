
using System.Net.Http.Json;

var http = new HttpClient();

http.DefaultRequestHeaders.Add("Client-ID", "8lihv1kzozi9iiq0nqxjk5wsrjlf45");
http.DefaultRequestHeaders.Add("Authorization", "Bearer blf6xjlxuvlxbqaq5vqdziobmf3931");

var res = await http.PostAsync("https://api.igdb.com/v4/games", new StringContent("limit 100;fields name,game_engines.*,websites.*;where name = \"Hunt: Showdown\";"));
var content = await res.Content.ReadAsStringAsync();

http.DefaultRequestHeaders.Clear();
res = await http.GetAsync("https://api.steampowered.com/ISteamUserStats/GetNumberOfCurrentPlayers/v1/?appid=594650");
content = await res.Content.ReadAsStringAsync();


res = await http.GetAsync("https://store.steampowered.com/appreviews/594650?json=1");
content = await res.Content.ReadAsStringAsync();


Console.ReadKey();