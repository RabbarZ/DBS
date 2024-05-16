using EngineTool.DataAccess;
using EngineTool.DataAccess.Repositories;
using EngineTool.Entities;
using Microsoft.Identity.Client;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineTool.Test.Repositories
{
    [TestClass]
    public class PlayerStatsRepositoryTests
    {
        private const int TimeoutInMS = 1000;
        private IRepository<PlayerStats>? repositoryMock;

        [TestInitialize]
        public void Setup()
        {
            this.repositoryMock = Substitute.For<IRepository<PlayerStats>>();
        }

        [TestMethod]
        public void Add_AddPlayerStatsToDatabase_AddsPlayerStats()
        {
            // Arrange
            Guid PlayerStatsId = Guid.NewGuid();
            var resetEvent = new AutoResetEvent(false);
            var playerStats = new PlayerStats() { Id = PlayerStatsId };

            Assert.IsNotNull(this.repositoryMock);
            this.repositoryMock.When(r => r.Add(Arg.Is<PlayerStats>(ps => ps.Id == PlayerStatsId)))
                .Do(x =>
            {
                resetEvent.Set();
            });
            var repository = new PlayerStatsRepository(this.repositoryMock);

            // Act
            repository.Add(playerStats);

            // Assert
            Assert.IsTrue(resetEvent.WaitOne(TimeoutInMS));
            this.repositoryMock.Received(1).Add(Arg.Any<PlayerStats>());
        }
    }
}
