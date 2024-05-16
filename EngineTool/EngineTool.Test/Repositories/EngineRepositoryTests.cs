using EngineTool.DataAccess.Entities;
using EngineTool.DataAccess.Interfaces;
using EngineTool.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace EngineTool.Test.Repositories
{
    [TestClass]
    public class EngineRepositoryTests
    {
        private IEngineContext? contextMock;

        [TestInitialize]
        public void Setup()
        {
            this.contextMock = Substitute.For<IEngineContext>();
        }

        [TestMethod]
        public void GetByIgdbId_FindsOneEngine_ReturnsEngine()
        {
            // Arrange
            const int IgdbId = 23;
            var engine1 = new Engine() { Name = "EngineName", IgdbId = IgdbId };
            var engines = new List<Engine>() { engine1 };

            var dbSetMock = Substitute.For<DbSet<Engine>>();
            dbSetMock.AsQueryable().Returns(engines.AsQueryable());

            Assert.IsNotNull(this.contextMock);
            this.contextMock.Set<Engine>().Returns(dbSetMock);

            var repository = new EngineRepository(this.contextMock);

            // Act
            var engine = repository.GetByIgdbId(IgdbId);

            // Assert
            Assert.IsNotNull(engine);
            Assert.AreEqual(engine1.IgdbId, engine.IgdbId);
            Assert.AreEqual(engine1.Name, engine.Name);
        }

        [TestMethod]
        public void GetByIgdbId_NoEnginesWithIgdbId_ReturnsNull()
        {
            // Arrange
            const int IgdbId1 = 23;
            const int IgdbId2 = 11;

            var engine1 = new Engine() { Name = "EngineName", IgdbId = IgdbId1 };
            var engines = new List<Engine> { engine1 };

            var dbSetMock = Substitute.For<DbSet<Engine>>();
            dbSetMock.AsQueryable().Returns(engines.AsQueryable());

            Assert.IsNotNull(this.contextMock);
            this.contextMock.Set<Engine>().Returns(dbSetMock);

            var repository = new EngineRepository(this.contextMock);

            // Act
            var engine = repository.GetByIgdbId(IgdbId2);

            // Assert
            Assert.IsNull(engine);
        }

        [TestMethod]
        public void GetByIgdbId_NoEnginesInRepository_ReturnsNull()
        {
            // Arrange
            const int IgdbId = 23;

            var engines = new List<Engine>();

            var dbSetMock = Substitute.For<DbSet<Engine>>();
            dbSetMock.AsQueryable().Returns(engines.AsQueryable());

            Assert.IsNotNull(this.contextMock);
            this.contextMock.Set<Engine>().Returns(dbSetMock);

            var repository = new EngineRepository(this.contextMock);

            // Act
            var engine = repository.GetByIgdbId(IgdbId);

            // Assert
            Assert.IsNull(engine);
        }

        [TestMethod]
        public void GetContainsGame_EngineContainsCorrectGame()
        {
            // Arrange
            var engineId = Guid.NewGuid();
            var gameId = Guid.NewGuid();

            var game = new Game() { Id = gameId, Name = "game1" };
            var games = new HashSet<Game>() { game };
            var engine = new Engine() { Id = engineId, Name = "test3", Games = games };
            var engines = new List<Engine>() { engine };

            var dbSetMock = Substitute.For<DbSet<Engine>>();
            dbSetMock.AsQueryable().Returns(engines.AsQueryable());

            Assert.IsNotNull(this.contextMock);
            this.contextMock.Set<Engine>().Returns(dbSetMock);

            var repository = new EngineRepository(this.contextMock);

            // Act
            var intersected = repository.GetContainsGame(engineId, gameId);

            // Assert
            Assert.IsTrue(intersected);
        }

        [TestMethod]
        public void GetContainsGame_EngineDoesNotContainCorrectGame()
        {
            // Arrange
            var engineId = Guid.NewGuid();
            var gameId = Guid.NewGuid();
            var otherId = Guid.NewGuid();

            var game = new Game() { Id = gameId, Name = "game1" };
            var games = new HashSet<Game>() { game };
            var engine = new Engine() { Id = engineId, Name = "test3", Games = games };
            var engines = new List<Engine>() { engine };

            var dbSetMock = Substitute.For<DbSet<Engine>>();
            dbSetMock.AsQueryable().Returns(engines.AsQueryable());

            Assert.IsNotNull(this.contextMock);
            this.contextMock.Set<Engine>().Returns(dbSetMock);

            var repository = new EngineRepository(this.contextMock);

            // Act
            var intersected = repository.GetContainsGame(engineId, otherId);

            // Assert
            Assert.IsFalse(intersected);
        }

        [TestMethod]
        public void GetContainsGame_EngineNotFound()
        {
            // Arrange
            var engineId = Guid.NewGuid();
            var gameId = Guid.NewGuid();
            var otherId1 = Guid.NewGuid();
            var otherId2 = Guid.NewGuid();

            var game = new Game() { Id = gameId, Name = "game1" };
            var games = new HashSet<Game>() { game };
            var engine = new Engine() { Id = engineId, Name = "test3", Games = games };
            var engines = new List<Engine>() { engine };

            var dbSetMock = Substitute.For<DbSet<Engine>>();
            dbSetMock.AsQueryable().Returns(engines.AsQueryable());

            Assert.IsNotNull(this.contextMock);
            this.contextMock.Set<Engine>().Returns(dbSetMock);

            var repository = new EngineRepository(this.contextMock);

            // Act
            var intersected = repository.GetContainsGame(otherId1, otherId2);

            // Assert
            Assert.IsNull(intersected);
        }
    }
}
