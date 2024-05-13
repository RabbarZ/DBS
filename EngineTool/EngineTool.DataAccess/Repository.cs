using EngineTool.DataAccess.Entities;
using EngineTool.Interfaces;

namespace EngineTool.DataAccess
{
    public class Repository<TEntity>(IEngineContext context) : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly IEngineContext context = context;

        public IQueryable<TEntity> GetAll()
        {
            return this.context.Set<TEntity>();
        }

        public TEntity? GetById(Guid id)
        {
            return this.context.Set<TEntity>().SingleOrDefault(x => x.Id == id);
        }

        public void Add(TEntity entity)
        {
            this.context.Set<TEntity>().Add(entity);
            this.context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            this.context.Set<TEntity>().Update(entity);
            this.context.SaveChanges();
        }

        public bool Delete(Guid id)
        {
            TEntity? entity = this.context.Set<TEntity>().SingleOrDefault(x => x.Id == id);
            if (entity == null)
            {
                return false;
            }

            this.context.Set<TEntity>().Remove(entity);
            this.context.SaveChanges();

            return true;
        }
    }
}
