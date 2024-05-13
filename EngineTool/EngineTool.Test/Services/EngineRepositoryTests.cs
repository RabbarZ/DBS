using EngineTool.DataAccess;
using EngineTool.DataAccess.Repositories;
using EngineTool.Entities;
using NSubstitute;

namespace EngineTool.Test.Repositories
{
    [TestClass]
    public class EngineRepositoryTests
    {
        private IRepository<Engine>? repositoryMock;

        [TestInitialize]
        public void Setup()
        {
            this.repositoryMock = Substitute.For<IRepository<Engine>>();
        }

        [TestMethod]
        public void GetByIgdbId_FindsOneEngine_ReturnsEngine()
        {
            // Arrange
            const int IgdbId = 23;

            Assert.IsNotNull(this.repositoryMock);

            var service = new EngineRepository(this.repositoryMock);

            var engine1 = new Engine() { Name = "EngineName", IgdbId = IgdbId };
            var engines = new List<Engine> { engine1 };

            this.repositoryMock.GetAll().Returns(engines.AsQueryable());

            // Act
            var engine = service.GetByIgdbId(IgdbId);

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

            Assert.IsNotNull(this.repositoryMock);
            var service = new EngineRepository(this.repositoryMock);

            var engine1 = new Engine() { Name = "EngineName", IgdbId = IgdbId1 };
            var engines = new List<Engine> { engine1 };

            this.repositoryMock.GetAll().Returns(engines.AsQueryable());

            // Act
            var engine = service.GetByIgdbId(IgdbId2);

            // Assert
            Assert.IsNull(engine);
        }


        [TestMethod]
        public void GetByIgdbId_NoEnginesInRepository_ReturnsNull()
        {
            // Arrange
            const int IgdbId = 23;

            Assert.IsNotNull(this.repositoryMock);

            var service = new EngineRepository(this.repositoryMock);
            var engines = new List<Engine>();

            this.repositoryMock.GetAll().Returns(engines.AsQueryable());

            // Act
            var engine = service.GetByIgdbId(IgdbId);

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

            Assert.IsNotNull(this.repositoryMock);
            var service = new EngineRepository(this.repositoryMock);

            this.repositoryMock.GetAll().Returns(engines.AsQueryable());

            // Act
            var intersected = service.GetContainsGame(engineId, gameId);

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

            Assert.IsNotNull(this.repositoryMock);
            var service = new EngineRepository(this.repositoryMock);

            this.repositoryMock.GetAll().Returns(engines.AsQueryable());

            // Act
            var intersected = service.GetContainsGame(engineId, otherId);

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

            Assert.IsNotNull(this.repositoryMock);
            var service = new EngineRepository(this.repositoryMock);

            this.repositoryMock.GetAll().Returns(engines.AsQueryable());

            // Act
            var intersected = service.GetContainsGame(otherId1, otherId2);

            // Assert
            Assert.IsNull(intersected);
        }

        [TestMethod]
        public void Add_AddsEngine()
        {
            // Arrange
            var engine = new Engine() { Name = "test4" };
            Assert.IsNotNull(this.repositoryMock);
            var service = new EngineRepository(this.repositoryMock);
            var resetEvent = new AutoResetEvent(false);

            this.repositoryMock.When(x => x.Add(Arg.Is<Engine>(e => e.Name == engine.Name)))
                .Do(x =>
                {
                    resetEvent.Set();
                });

            // Act
            service.Add(engine);

            // Assert
            Assert.IsTrue(resetEvent.WaitOne(1000));
        }

        [TestMethod]
        public void Update_UpdatesEngine()
        {
            // Arrange
            var engine = new Engine() { Name = "test4" };
            Assert.IsNotNull(this.repositoryMock);
            var service = new EngineRepository(this.repositoryMock);
            var resetEvent = new AutoResetEvent(false);

            this.repositoryMock.When(x => x.Update(Arg.Is<Engine>(e => e.Name == engine.Name)))
                .Do(x =>
                {
                    resetEvent.Set();
                });

            // Act
            service.Update(engine);

            // Assert
            Assert.IsTrue(resetEvent.WaitOne(1000));
        }
    }
}
