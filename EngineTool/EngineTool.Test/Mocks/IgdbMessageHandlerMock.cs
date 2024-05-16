using EngineTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace EngineTool.Test.Mocks;

internal class IgdbMessageHandlerMock(IgdbGame[] games) : HttpMessageHandler
{
    private readonly IgdbGame[] _games = games;


    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var stringContent = await request.Content.ReadAsStringAsync();
        string[] parameters = stringContent.Split(';');

        int offset = int.Parse(parameters.Single(p => p.StartsWith("offset")).Split(' ')[1]);
        int limit = int.Parse(parameters.Single(p => p.StartsWith(" limit")).Split(' ')[2]);

        IgdbGame[] gamesToReturn = _games.Skip(offset).Take(limit).ToArray();

        var httpResponseMessage = new HttpResponseMessage
        {
            Content = JsonContent.Create(gamesToReturn)
        };

        return await Task.FromResult(httpResponseMessage);
    }
}