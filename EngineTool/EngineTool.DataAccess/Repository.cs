using EngineTool.DataAccess.Entities;
using EngineTool.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EngineTool.DataAccess;

internal class Repository<TEntity>(IEngineContext context) : IRepository<TEntity> where TEntity : class, IEntity
{
    private readonly IEngineContext _context = context;
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    protected IQueryable<TEntity> GetQueryable()
    {
        return _dbSet.AsQueryable();
    }

    public TEntity? GetById(Guid id)
    {
        return _dbSet.Find(id);
    }

    public void Add(TEntity entity)
    {
        _dbSet.Add(entity);
        _context.SaveChanges();
    }

    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
        _context.SaveChanges();
    }

    public bool Delete(Guid id)
    {
        TEntity? entity = GetById(id);
        if (entity == null)
        {
            return false;
        }

        _dbSet.Remove(entity);
        _context.SaveChanges();

        return true;
    }
}