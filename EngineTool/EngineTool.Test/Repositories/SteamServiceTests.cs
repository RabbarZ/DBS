using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using EngineTool.Models;
using EngineTool.Services;
using EngineTool.Test.Mocks;

namespace EngineTool.Test.Repositories;

[TestClass]
public class SteamServiceTests
{
    [TestMethod]
    public async Task GetCurrentPlayerCountAsync_HttpRequestException_ReturnsNull()
    {
        // Arrange
        var httpMessageHandler = new HttpMessageHandlerMock(exception: new HttpRequestException());
        var httpClient = new HttpClient(httpMessageHandler);
        var steamService = new SteamService(httpClient);

        // Act
        int? currentPlayerCount = await steamService.GetCurrentPlayerCountAsync(0);

        // Assert
        Assert.IsNull(currentPlayerCount);
    }

    [TestMethod]
    public async Task GetCurrentPlayerCountAsync_BadGateway_ReturnsNull()
    {
        // Arrange
        var httpResponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
        var httpMessageHandler = new HttpMessageHandlerMock(httpResponse);
        var httpClient = new HttpClient(httpMessageHandler);
        var steamService = new SteamService(httpClient);

        // Act
        int? currentPlayerCount = await steamService.GetCurrentPlayerCountAsync(0);

        // Assert
        Assert.IsNull(currentPlayerCount);
    }

    [TestMethod]
    [ExpectedException(typeof(JsonException))]
    public async Task GetCurrentPlayerCountAsync_OK_NoContent_ThrowsJsonException()
    {
        // Arrange
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
        var httpMessageHandler = new HttpMessageHandlerMock(httpResponse);
        var httpClient = new HttpClient(httpMessageHandler);
        var steamService = new SteamService(httpClient);

        // Act
        await steamService.GetCurrentPlayerCountAsync(0);
    }

    [TestMethod]
    public async Task GetCurrentPlayerCountAsync_OK_ReturnsCurrentPlayerCount()
    {
        // Arrange
        var playerStatsResponse = new SteamPlayerStatsResponse
        {
            PlayerStats = new SteamPlayerStats
            {
                Success = 1,
                PlayerCount = 500
            }
        };

        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(playerStatsResponse)
        };

        var httpMessageHandler = new HttpMessageHandlerMock(httpResponse);
        var httpClient = new HttpClient(httpMessageHandler);
        var steamService = new SteamService(httpClient);

        // Act
        int? currentPlayerCount = await steamService.GetCurrentPlayerCountAsync(0);

        // Assert
        Assert.AreEqual(playerStatsResponse.PlayerStats.PlayerCount, currentPlayerCount);
    }

    [TestMethod]
    [ExpectedException(typeof(JsonException))]
    public async Task GetCurrentPlayerCountAsync_OK_FaultyContent_ThrowsJsonException()
    {
        // Arrange
        const int playerStatsResponse = 500;

        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(playerStatsResponse)
        };

        var httpMessageHandler = new HttpMessageHandlerMock(httpResponse);
        var httpClient = new HttpClient(httpMessageHandler);
        var steamService = new SteamService(httpClient);

        // Act
        await steamService.GetCurrentPlayerCountAsync(0);
    }
}