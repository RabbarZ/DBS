using EngineTool.DataAccess.Entities;

namespace EngineTool.DataAccess
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        IQueryable<TEntity> GetAll();

        TEntity? GetById(Guid id);

        void Add(TEntity entity);

        void Update(TEntity entity);

        bool Delete(Guid id);
    }
}
