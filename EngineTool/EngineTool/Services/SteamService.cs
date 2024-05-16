using EngineTool.Interfaces;
using EngineTool.Models;
using System.Net.Http.Json;

namespace EngineTool.Services;

public class SteamService(HttpClient httpClient) : ISteamService
{
    private readonly HttpClient _http = httpClient;

    public async Task<int?> GetCurrentPlayerCountAsync(int steamAppId)
    {
        try
        {
            var response = await _http.GetFromJsonAsync<SteamPlayerStatsResponse>($"https://api.steampowered.com/ISteamUserStats/GetNumberOfCurrentPlayers/v1/?appid={steamAppId}");
            return response?.PlayerStats.PlayerCount;
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    public async Task<SteamRating?> GetRatingAsync(int steamAppId)
    {
        try
        {
            var response = await _http.GetFromJsonAsync<SteamQuerySummary>($"https://store.steampowered.com/appreviews/{steamAppId}?json=1&num_per_page=0");
            return response?.Rating;
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }
}