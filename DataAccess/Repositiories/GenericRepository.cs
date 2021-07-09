﻿using Microsoft.EntityFrameworkCore;
using Movies.Data.DataAccess;
using MoviesDataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MoviesDataLayer
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal MoviesDBContext context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(MoviesDBContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }


            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual TEntity GetByID(object id)
        {            
            return dbSet.Find(id);
            
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);             
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);                
            }            
            dbSet.Remove(entityToDelete);           
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public async Task DeleteAsync(object id)
        {
            var entityToDelete = await dbSet.FindAsync(id);
            Delete(entityToDelete);
        }

        public async Task<TEntity> GetByIDAsync(object id)
        {            
            var entity = await dbSet.FindAsync(id);
            return entity;
        }

        public Task InsertAsync(TEntity entity)
        {
            return dbSet.AddAsync(entity).AsTask();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var items = await dbSet.ToListAsync();
            return items;
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
