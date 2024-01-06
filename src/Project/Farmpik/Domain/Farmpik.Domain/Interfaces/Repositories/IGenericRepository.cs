/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using Farmpik.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Farmpik.Domain.Interfaces.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetAsync(Guid id, bool onlyActive = true);

        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter);

        Task<IEnumerable<TEntity>> GetAllAsync(bool onlyActive = true);

        Task<PaginatedList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null,
            int pageNumber = 1, int pageSize = int.MaxValue,
            Expression<Func<TEntity, object>> orderBy = null, bool sortbyAsce = true);

        Task<TEntity> AddAsync(TEntity entity);

        Task<bool> DeleteAsync(Guid id);

        Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> filter);

        Task<bool> UpdateAsync(TEntity entity);

        bool BulkSave(string tablename, List<TEntity> entities, Guid createdBy, string conditions = null);
    }
}