﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MoviesDataLayer.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity: class
    {
        void Delete(TEntity entityToDelete);
        void Delete(object id);
        Task DeleteAsync(object id);
        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");
        TEntity GetByID(object id);
        Task<TEntity> GetByIDAsync(object id);
        void Insert(TEntity entity);
        Task InsertAsync(TEntity entity);
        void Update(TEntity entityToUpdate);
    }
}
