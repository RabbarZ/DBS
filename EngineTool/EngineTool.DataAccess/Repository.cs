﻿using EngineTool.DataAccess.Entities;
using EngineTool.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EngineTool.DataAccess
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly IEngineContext context;

        public Repository(IEngineContext context)
        {
            this.context = context;
        }

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
