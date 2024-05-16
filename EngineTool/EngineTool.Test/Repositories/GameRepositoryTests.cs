using EngineTool.DataAccess;
using EngineTool.DataAccess.Repositories;
using EngineTool.Entities;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineTool.Test.Repositories
{
    [TestClass]
    public class GameRepositoryTests
    {
        private IRepository<Game>? repositoryMock;

        [TestInitialize]
        public void Setup()
        {
            this.repositoryMock = Substitute.For<IRepository<Game>>();
        }

        [TestMethod]
        public void GetByIgdbId_FindsOneGame_ReturnsGame()
        {
            // Arrange
            const int IgdbId = 23;

            Assert.IsNotNull(this.repositoryMock);
            var repository = new GameRepository(this.repositoryMock);

            var game1 = new Game() { Name = "the witcher", IgdbId = IgdbId };
            var games = new List<Game>() { game1 };

            this.repositoryMock.GetAll().Returns(games.AsQueryable());

            // Act
            var result = repository.GetByIgdbId(IgdbId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(game1.IgdbId, result.IgdbId);
            Assert.AreEqual(game1.Name, result.Name);
        }

        [TestMethod]
        public void GetByIgdbId_NoGameWithIgdbId_ReturnsNull()
        {
            // Arrange
            const int IgdbId1 = 23;
            const int IgdbId2 = 24;

            Assert.IsNotNull(this.repositoryMock);
            var repository = new GameRepository(this.repositoryMock);

            var game1 = new Game() { Name = "the witcher", IgdbId = IgdbId1 };
            var games = new List<Game>() { game1 };

            this.repositoryMock.GetAll().Returns(games.AsQueryable());

            // Act
            var result = repository.GetByIgdbId(IgdbId2);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetById_NoGameInRepository_ReturnsNull()
        {
            // Arrange
            const int IgdbId = 23;

            var games = new List<Game>();

            Assert.IsNotNull(this.repositoryMock);
            this.repositoryMock.GetAll().Returns(games.AsQueryable());
            var repository = new GameRepository(this.repositoryMock);

            // Act
            var result = repository.GetByIgdbId(IgdbId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Add_AddNewGameToDatabase_AddsGame()
        {
            // Arrange
            var resetEvent = new AutoResetEvent(false);
            var game = new Game() { Name = "the witcher 2" };

            Assert.IsNotNull(this.repositoryMock);
            var repository = new GameRepository(this.repositoryMock);
            this.repositoryMock.When(rm => rm.Add(Arg.Is<Game>(game => game.Name == game.Name)))
                .Do(x =>
                {
                    resetEvent.Set();
                });

            // Act
            repository.Add(game);

            // Assert
            Assert.IsTrue(resetEvent.WaitOne(1000));
        }
    }
}
