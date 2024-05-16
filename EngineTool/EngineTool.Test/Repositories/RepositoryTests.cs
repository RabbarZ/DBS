using EngineTool.DataAccess;
using EngineTool.DataAccess.Entities;
using EngineTool.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace EngineTool.Test.Repositories
{
    [TestClass]
    public class RepositoryTests
    {
        private IEngineContext? contextMock;

        [TestInitialize]
        public void Setup()
        {
            this.contextMock = Substitute.For<IEngineContext>();
        }

        [TestMethod]
        public void GetById_OneEngineFoundInDatabase_ReturnsEngine()
        {
            // Arrange
            var id = Guid.NewGuid();
            var engine = new Engine() { Id = id, Name = "Unity" };

            var dbSetMock = Substitute.For<DbSet<Engine>>();
            dbSetMock.Find(id).Returns(engine);

            Assert.IsNotNull(this.contextMock);
            this.contextMock.Set<Engine>().Returns(dbSetMock);

            var repository = new Repository<Engine>(this.contextMock);

            // Act
            var result = repository.GetById(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
        }

        [TestMethod]
        public void GetById_NoEngineFoundInDatabase_ReturnsEngine()
        {
            // Arrange
            var id = Guid.NewGuid();

            var dbSetMock = Substitute.For<DbSet<Engine>>();

            Assert.IsNotNull(this.contextMock);
            this.contextMock.Set<Engine>().Returns(dbSetMock);

            var repository = new Repository<Engine>(this.contextMock);

            // Act
            var result = repository.GetById(id);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Add_AddNewEngineToDatabase_ReicevesAddOnDbSet()
        {
            // Arrange
            var engine = new Engine() { Name = "Unity" };

            var dbSetMock = Substitute.For<DbSet<Engine>>();

            Assert.IsNotNull(this.contextMock);
            this.contextMock.Set<Engine>().Returns(dbSetMock);

            var repository = new Repository<Engine>(this.contextMock);

            // Act
            repository.Add(engine);

            // Assert
            dbSetMock.Received(1).Add(Arg.Is<Engine>(e => e.Name == engine.Name));
            this.contextMock.Received(1).SaveChanges();
        }

        [TestMethod]
        public void Update_UpdateEngineInDatabase_ReicevesAddOnDbSet()
        {
            // Arrange
            var engine = new Engine() { Name = "Unity" };

            var dbSetMock = Substitute.For<DbSet<Engine>>();

            Assert.IsNotNull(this.contextMock);
            this.contextMock.Set<Engine>().Returns(dbSetMock);

            var repository = new Repository<Engine>(this.contextMock);

            // Act
            repository.Update(engine);

            // Assert
            dbSetMock.Received(1).Update(Arg.Is<Engine>(e => e.Name == engine.Name));
            this.contextMock.Received(1).SaveChanges();
        }

        [TestMethod]
        public void Delete_DeleteEngineFromDatabase_EngineRemoved()
        {
            // Arrange
            var id = Guid.NewGuid();
            var engine = new Engine() { Id = id, Name = "Unity" };

            var dbSetMock = Substitute.For<DbSet<Engine>>();
            dbSetMock.Find(id).Returns(engine);

            Assert.IsNotNull(this.contextMock);
            this.contextMock.Set<Engine>().Returns(dbSetMock);

            var repository = new Repository<Engine>(this.contextMock);

            // Act
            repository.Delete(id);

            // Assert
            dbSetMock.Received(1).Remove(Arg.Is<Engine>(e => e.Id == engine.Id));
            this.contextMock.Received(1).SaveChanges();
        }

        [TestMethod]
        public void Delete_NoEngineWithExpectedIdInDatabase_NoEngineDeleted()
        {
            // Arrange
            var id = Guid.NewGuid();

            var dbSetMock = Substitute.For<DbSet<Engine>>();

            Assert.IsNotNull(this.contextMock);
            this.contextMock.Set<Engine>().Returns(dbSetMock);

            var repository = new Repository<Engine>(this.contextMock);

            // Act
            repository.Delete(id);

            // Assert
            dbSetMock.Received(0).Remove(Arg.Any<Engine>());
            this.contextMock.Received(0).SaveChanges();
        }
    }
}
