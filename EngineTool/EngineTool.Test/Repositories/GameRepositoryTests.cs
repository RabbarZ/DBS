using EngineTool.DataAccess.Entities;
using EngineTool.DataAccess.Interfaces;
using EngineTool.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace EngineTool.Test.Repositories
{
    [TestClass]
    public class GameRepositoryTests
    {
        private IEngineContext? contextMock;

        [TestInitialize]
        public void Setup()
        {
            this.contextMock = Substitute.For<IEngineContext>();
        }

        [TestMethod]
        public void GetByIgdbId_FindsOneGame_ReturnsGame()
        {
            // Arrange
            const int IgdbId = 23;

            var game1 = new Game() { Name = "the witcher", IgdbId = IgdbId };
            var games = new List<Game>() { game1 };

            var dbSetMock = Substitute.For<DbSet<Game>>();
            dbSetMock.AsQueryable().Returns(games.AsQueryable());

            Assert.IsNotNull(this.contextMock);
            this.contextMock.Set<Game>().Returns(dbSetMock);

            var repository = new GameRepository(this.contextMock);

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

            var game1 = new Game() { Name = "the witcher", IgdbId = IgdbId1 };
            var games = new List<Game>() { game1 };

            var dbSetMock = Substitute.For<DbSet<Game>>();
            dbSetMock.AsQueryable().Returns(games.AsQueryable());

            Assert.IsNotNull(this.contextMock);
            this.contextMock.Set<Game>().Returns(dbSetMock);

            var repository = new GameRepository(this.contextMock);

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

            var dbSetMock = Substitute.For<DbSet<Game>>();
            dbSetMock.AsQueryable().Returns(games.AsQueryable());

            Assert.IsNotNull(this.contextMock);
            this.contextMock.Set<Game>().Returns(dbSetMock);

            var repository = new GameRepository(this.contextMock);

            // Act
            var result = repository.GetByIgdbId(IgdbId);

            // Assert
            Assert.IsNull(result);
        }
        /*
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
        }*/
    }
}
