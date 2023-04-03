
using EngineTool.Models;
using EngineTool.Services;
using System.Net.Http.Json;

var igdbService = new IgdbService();
var steamService = new SteamService();

IgdbGame[] games = await igdbService.GetGamesAsync(500);
foreach (var game in games)
{
    var steamUrl = game.Websites.Single(w => w.Category == 13).Url;
    var steamId = steamUrl.Split('/').Last();

    var rating = await steamService.GetRatingAsync(steamId);
    var playerCount = await steamService.GetCurrentPlayerCountAsync(steamId);
}
Console.ReadKey();