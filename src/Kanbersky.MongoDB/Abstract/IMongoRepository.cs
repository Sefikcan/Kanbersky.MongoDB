using Kanbersky.MongoDB.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Kanbersky.MongoDB.Abstract
{
    public interface IMongoRepository<TEntity>
        where TEntity : BaseMongoEntity, new()
    {
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> filter);

        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter);

        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> filter, int pageIndex, int size);

        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter, int pageIndex, int size);

        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order, int pageIndex, int size);

        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order, int pageIndex, int size);

        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order, int pageIndex, int size, bool isDescending);

        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order, int pageIndex, int size, bool isDescending);

        IEnumerable<TEntity> FindAll();

        Task<IEnumerable<TEntity>> FindAllAsync();

        IEnumerable<TEntity> FindAll(int pageIndex, int size);

        Task<IEnumerable<TEntity>> FindAllAsync(int pageIndex, int size);

        IEnumerable<TEntity> FindAll(Expression<Func<TEntity, object>> order, int pageIndex, int size);

        Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, object>> order, int? pageIndex = 0, int? size = 10);

        IEnumerable<TEntity> FindAll(Expression<Func<TEntity, object>> order, int pageIndex, int size, bool isDescending);

        Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, object>> order, int pageIndex, int size, bool isDescending);

        bool Any(Expression<Func<TEntity, bool>> filter);

        TEntity Last();

        TEntity Last(Expression<Func<TEntity, bool>> filter);

        TEntity Last(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order);

        TEntity Last(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order, bool isDescending);

        TEntity First();

        Task<TEntity> FirstAsync();

        TEntity First(Expression<Func<TEntity, bool>> filter);

        Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> filter);

        TEntity First(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order);

        Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order);

        TEntity First(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order, bool isDescending);

        Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order, bool isDescending);

        TEntity GetById(string id);

        Task<TEntity> GetByIdAsync(string id);

        bool Update<TField>(TEntity entity, Expression<Func<TEntity, TField>> field, TField value);

        Task<bool> UpdateAsync<TField>(TEntity entity, Expression<Func<TEntity, TField>> field, TField value);

        bool Update(string id, params UpdateDefinition<TEntity>[] updates);

        Task<bool> UpdateAsync(string id, params UpdateDefinition<TEntity>[] updates);

        bool Update(TEntity entity, params UpdateDefinition<TEntity>[] updates);

        Task<bool> UpdateAsync(TEntity entity, params UpdateDefinition<TEntity>[] updates);

        bool Update<TField>(FilterDefinition<TEntity> filter, Expression<Func<TEntity, TField>> field, TField value);

        Task<bool> UpdateAsync<TField>(FilterDefinition<TEntity> filter, Expression<Func<TEntity, TField>> field, TField value);

        bool Update(FilterDefinition<TEntity> filter, params UpdateDefinition<TEntity>[] updates);

        Task<bool> UpdateAsync(FilterDefinition<TEntity> filter, params UpdateDefinition<TEntity>[] updates);

        bool Update(Expression<Func<TEntity, bool>> filter, params UpdateDefinition<TEntity>[] updates);

        Task<bool> UpdateAsync(Expression<Func<TEntity, bool>> filter, params UpdateDefinition<TEntity>[] updates);

        void Delete(string id);

        Task DeleteAsync(string id);

        void Delete(Expression<Func<TEntity, bool>> filter);

        Task DeleteAsync(Expression<Func<TEntity, bool>> filter);

        void Insert(TEntity entity);

        Task InsertAsync(TEntity entity);

        void Insert(IEnumerable<TEntity> entities);

        Task InsertAsync(IEnumerable<TEntity> entities);
    }
}
