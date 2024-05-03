using EngineTool.Config;
using EngineTool.Models;
using EngineTool.Services;
using Microsoft.Extensions.Configuration;

namespace EngineTool.Test
{
    [TestClass]
    public class IgdbServiceTests
    {
        private AppSettings? appSettings;

        [TestInitialize]
        public void Initialize()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            IConfiguration config = builder.Build();
            this.appSettings = config.Get<AppSettings>();
        }

        [TestMethod]
        public void GetGamesAsync_Get500Game_Receive500Game()
        {
            // Arrange
            const int GameCount = 500;
            var igdbService = new IgdbService(this.appSettings);

            // Act
            var task = igdbService.GetGamesAsync(GameCount);
            task.Wait();
            var games = task.Result;

            // Assert
            Assert.Equals(GameCount, games.Count);
        }

        [TestMethod]
        public async Task GetGamesAsync_Get5Games_Receive5Games()
        {
            // Arrange
            const int GameCount = 5;
            var httpClientMock = Substitute.For<HttpClient>();
            var igdbService = new IgdbService(this.appSettings, new HttpClient());

            // Act
            var games = await igdbService.GetGamesAsync(GameCount);

            // Assert
            Assert.Equals(GameCount, games.Count);
        }
    }
}