using EngineTool.DataAccess.Entities;

namespace EngineTool.DataAccess.Interfaces;

public interface IRepository<TEntity> where TEntity : IEntity
{
    TEntity? GetById(Guid id);

    void Add(TEntity entity);

    void Update(TEntity entity);

    bool Delete(Guid id);
}