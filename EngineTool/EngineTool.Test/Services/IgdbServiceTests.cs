using EngineTool.Config;
using EngineTool.Models;
using EngineTool.Services;
using EngineTool.Test.Mocks;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineTool.Test.Services
{
    [TestClass]
    public class IgdbServiceTests
    {
        [TestMethod]
        public void GetSteamId_ReturnsSteamId()
        {
            // Arrange
            var expectedSteamId = 211740;
            var game = new IgdbGame
            {
                Id = 0,
                Name = "Test",
                Engines = new HashSet<IgdbEngine>(),
                Websites = new HashSet<IgdbWebsite>()
                {
                    new()
                    {
                        Category = 13,
                        Url = $"https://store.steampowered.com/app/{expectedSteamId}"
                    }
                }
            };

            var igdbService = new IgdbService(Substitute.For<IOptions<IgdbApiSettings>>(), Substitute.For<HttpClient>());


            // Act
            int? steamId = igdbService.GetSteamId(game);

            // Assert
            Assert.AreEqual(expectedSteamId, steamId);
        }

        [TestMethod]
        public void GetSteamId_NoSteamWebsite_ReturnsNull()
        {
            // Arrange
            int? expectedSteamId = null;
            var game = new IgdbGame
            {
                Id = 0,
                Name = "Test",
                Engines = new HashSet<IgdbEngine>(),
                Websites = new HashSet<IgdbWebsite>()
                {
                    new()
                    {
                        Category = 1,
                        Url = "https://store.steampowered.com/app/211740"
                    }
                }
            };

            var igdbService = new IgdbService(Substitute.For<IOptions<IgdbApiSettings>>(), Substitute.For<HttpClient>());


            // Act
            int? steamId = igdbService.GetSteamId(game);

            // Assert
            Assert.AreEqual(expectedSteamId, steamId);
        }

        [TestMethod]
        public void GetSteamId_FaultySteamWebsite_ReturnsNull()
        {
            // Arrange
            int? expectedSteamId = null;
            var game = new IgdbGame
            {
                Id = 0,
                Name = "Test",
                Engines = new HashSet<IgdbEngine>(),
                Websites = new HashSet<IgdbWebsite>()
                {
                    new()
                    {
                        Category = 13,
                        Url = "https://store.steampowered.com/app/faulty/211740"
                    }
                }
            };

            var igdbService = new IgdbService(Substitute.For<IOptions<IgdbApiSettings>>(), Substitute.For<HttpClient>());


            // Act
            int? steamId = igdbService.GetSteamId(game);

            // Assert
            Assert.AreEqual(expectedSteamId, steamId);
        }

        [TestMethod]
        public void GetGamesAsync_NoCount_ReturnsAll()
        {
            // Arrange
            IgdbGame[] expectedGames = Enumerable.Range(0, 1000).Select(x => new IgdbGame
            {
                Id = x,
                Name = $"Test{x}",
                Engines = new HashSet<IgdbEngine>(),
                Websites = new HashSet<IgdbWebsite>()
            }).ToArray();

            var httpMessageHandler = new IgdbMessageHandlerMock(expectedGames);
            var httpClient = new HttpClient(httpMessageHandler);

            var apiSettingsMock = Substitute.For<IgdbApiSettings>();
            apiSettingsMock.Url = "https://testurl.com/games";

            var apiSettingsOptionsMock = Substitute.For<IOptions<IgdbApiSettings>>();
            apiSettingsOptionsMock.Value.Returns(apiSettingsMock);

            var igdbService = new IgdbService(apiSettingsOptionsMock, httpClient);

            // Act
            var actualGames = igdbService.GetGamesAsync().ToBlockingEnumerable().ToArray();

            // Assert
            Assert.AreEqual(expectedGames.Length, actualGames.Length);
        }

        [TestMethod]
        public void GetGamesAsync_WithCount_ReturnsOnlyCountAmountOfGames()
        {
            // Arrange
            IgdbGame[] games = Enumerable.Range(0, 1000).Select(x => new IgdbGame
            {
                Id = x,
                Name = $"Test{x}",
                Engines = new HashSet<IgdbEngine>(),
                Websites = new HashSet<IgdbWebsite>()
            }).ToArray();

            var expectedLength = 50;

            var httpMessageHandler = new IgdbMessageHandlerMock(games);
            var httpClient = new HttpClient(httpMessageHandler);

            var apiSettingsMock = Substitute.For<IgdbApiSettings>();
            apiSettingsMock.Url = "https://testurl.com/games";

            var apiSettingsOptionsMock = Substitute.For<IOptions<IgdbApiSettings>>();
            apiSettingsOptionsMock.Value.Returns(apiSettingsMock);

            var igdbService = new IgdbService(apiSettingsOptionsMock, httpClient);

            // Act
            var actualGames = igdbService.GetGamesAsync(expectedLength).ToBlockingEnumerable().ToArray();

            // Assert
            Assert.AreEqual(expectedLength, actualGames.Length);
        }

        [TestMethod]
        public void GetGamesAsync_CountGreaterThanGames_ReturnsOnlyAmountOfGames()
        {
            // Arrange
            IgdbGame[] games = Enumerable.Range(0, 1000).Select(x => new IgdbGame
            {
                Id = x,
                Name = $"Test{x}",
                Engines = new HashSet<IgdbEngine>(),
                Websites = new HashSet<IgdbWebsite>()
            }).ToArray();

            var httpMessageHandler = new IgdbMessageHandlerMock(games);
            var httpClient = new HttpClient(httpMessageHandler);

            var apiSettingsMock = Substitute.For<IgdbApiSettings>();
            apiSettingsMock.Url = "https://testurl.com/games";

            var apiSettingsOptionsMock = Substitute.For<IOptions<IgdbApiSettings>>();
            apiSettingsOptionsMock.Value.Returns(apiSettingsMock);

            var igdbService = new IgdbService(apiSettingsOptionsMock, httpClient);

            // Act
            var actualGames = igdbService.GetGamesAsync(20000).ToBlockingEnumerable().ToArray();

            // Assert
            Assert.AreEqual(games.Length, actualGames.Length);
        }
    }
}
