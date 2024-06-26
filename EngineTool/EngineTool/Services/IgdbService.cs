﻿using EngineTool.Config;
using EngineTool.Interfaces;
using EngineTool.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace EngineTool.Services;

internal sealed class IgdbService(IOptions<IgdbApiSettings> igdbApiSettings, HttpClient httpClient) : IIgdbService
{
    private const int MaxCount = 500;
    private readonly HttpClient _http = httpClient;
    private readonly IgdbApiSettings _igdbApiSettings = igdbApiSettings.Value;

    public async IAsyncEnumerable<IgdbGame> GetGamesAsync(int count)
    {
        int limit = MaxCount;
        if (count < MaxCount)
        {
            limit = count;
        }

        for (var i = 0; i < count / limit; i++)
        {
            int offset = i * limit;

            HttpResponseMessage response = await _http.PostAsync(_igdbApiSettings.Url, new StringContent($"offset {offset}; limit {limit};fields name,game_engines.name,game_engines.id,websites.category,websites.url;where websites.category = 13 & game_engines != null & category = (0, 8, 9);"));
            IgdbGame[]? games = await response.Content.ReadFromJsonAsync<IgdbGame[]>();
            if (games == null)
            {
                continue;
            }

            foreach (IgdbGame game in games)
            {
                yield return game;
            }
        }
    }

    public async IAsyncEnumerable<IgdbGame> GetGamesAsync()
    {
        IgdbGame[]? games = [];
        var offset = 0;
        while (games != null)
        {
            HttpResponseMessage response = await _http.PostAsync(_igdbApiSettings.Url, new StringContent($"offset {offset}; limit {MaxCount};fields name,game_engines.name,game_engines.id,websites.category,websites.url;where websites.category = 13 & game_engines != null & category = (0, 8, 9);"));
            games = await response.Content.ReadFromJsonAsync<IgdbGame[]>();
            offset += MaxCount;

            if (games == null)
            {
                continue;
            }

            foreach (IgdbGame game in games)
            {
                yield return game;
            }

            if (games.Length < 1)
            {
                yield break;
            }
        }
    }

    public int? GetSteamId(IgdbGame game)
    {
        string? steamUrl = game.Websites.FirstOrDefault(w => w.Category == 13)?.Url;
        if (steamUrl == null)
        {
            return null;
        }

        string[] steamUrlSubstrings = steamUrl.Split('/');
        if (steamUrlSubstrings.Length < 5)
        {
            return null;
        }

        if (int.TryParse(steamUrlSubstrings[4], out int steamId))
        {
            return steamId;
        }

        return null;
    }
}