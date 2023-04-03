
using EngineTool.Models;
using EngineTool.Services;
using System.Net.Http.Json;

var igdbService = new IgdbService();
Game[] games = await igdbService.GetGamesAsync(500);

foreach (var game in games)
{
    var steamUrl = game.Websites.Single(w => w.Category == 13).Url;
    var steamId = steamUrl.Split('/').Last();


}
//http.DefaultRequestHeaders.Clear();
//res = await http.GetASJson("https://api.steampowered.com/ISteamUserStats/GetNumberOfCurrentPlayers/v1/?appid=594650");
//content = await res.Content.ReadAsStringAsync();


//res = await http.GetAsync("https://store.steampowered.com/appreviews/594650?json=1");
//content = await res.Content.ReadAsJs();


Console.ReadKey();