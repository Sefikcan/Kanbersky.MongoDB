using Kanbersky.MongoDB.Abstract;
using Kanbersky.MongoDB.Models;
using Kanbersky.MongoDB.Settings;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Kanbersky.MongoDB.Concrete
{
    public class MongoRepository<TEntity> : IMongoRepository<TEntity>
        where TEntity: BaseMongoEntity, new()
    {
        private readonly IMongoCollection<TEntity> _mongoCollection;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="databaseName"></param>
        public MongoRepository(MongoDBSettings settings)
        {
            var client = new MongoClient(settings.ConnectionStrings);
            var database = client.GetDatabase(settings.DatabaseName);
            _mongoCollection = database.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        #region methods

        #region Delete Operations

        /// <summary>
        /// Remove record by id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(string id)
        {
            _mongoCollection.DeleteOne(i => i.Id == id);
        }

        /// <summary>
        /// Remove record by id
        /// </summary>
        /// <param name="id"></param>
        public async Task DeleteAsync(string id)
        {
            await _mongoCollection.DeleteOneAsync(i => i.Id == id);
        }

        /// <summary>
        /// Remove record by specific filter
        /// </summary>
        /// <param name="filter"></param>
        public void Delete(Expression<Func<TEntity, bool>> filter)
        {
            _mongoCollection.DeleteMany(filter);
        }

        /// <summary>
        /// Remove record by specific filter
        /// </summary>
        /// <param name="filter"></param>
        public async Task DeleteAsync(Expression<Func<TEntity, bool>> filter)
        {
            await _mongoCollection.DeleteManyAsync(filter);
        }

        #endregion

        #region Insert Operations

       /// <summary>
       /// Insert record
       /// </summary>
       /// <param name="entity"></param>
        public void Insert(TEntity entity)
        {
            _mongoCollection.InsertOne(entity);
        }

        /// <summary>
        /// Insert record
        /// </summary>
        /// <param name="entity"></param>
        public async Task InsertAsync(TEntity entity)
        {
            await _mongoCollection.InsertOneAsync(entity);
        }

        /// <summary>
        /// Insert records
        /// </summary>
        /// <param name="entities"></param>
        public void Insert(IEnumerable<TEntity> entities)
        {
            _mongoCollection.InsertMany(entities);
        }

        /// <summary>
        /// Insert records
        /// </summary>
        /// <param name="entities"></param>
        public async Task InsertAsync(IEnumerable<TEntity> entities)
        {
            await _mongoCollection.InsertManyAsync(entities);
        }

        #endregion

        #region GetById Operations

       /// <summary>
       /// Get Record by id
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
        public TEntity GetById(string id)
        {
            return _mongoCollection.Find(i => i.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Get Record by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TEntity> GetByIdAsync(string id)
        {
            var result = await _mongoCollection.FindAsync(i => i.Id == id);
            return result.FirstOrDefault();
        }

        #endregion

        #region Find Operations

        /// <summary>
        /// Find record by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> filter)
        {
            return _mongoCollection.Find(filter).ToList();
        }

        /// <summary>
        /// Find record by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _mongoCollection.Find(filter).ToListAsync();
        }

        /// <summary>
        /// Find pageable record by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pageIndex"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> filter, int pageIndex, int size)
        {
            return Find(filter, i => i.Id, pageIndex, size);
        }

        /// <summary>
        /// Find pageable record by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pageIndex"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter, int pageIndex, int size)
        {
            return await FindAsync(filter, i => i.Id, pageIndex, size);
        }

        /// <summary>
        /// Find pageable record by filter and order
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="order"></param>
        /// <param name="pageIndex"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order, int pageIndex, int size)
        {
            return Find(filter, order, pageIndex, size, true);
        }

        /// <summary>
        ///  Find pageable record by filter and order
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="order"></param>
        /// <param name="pageIndex"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order, int pageIndex, int size)
        {
            return await FindAsync(filter, order, pageIndex, size, true);
        }

        /// <summary>
        ///  Find pageable record by filter and order
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="order"></param>
        /// <param name="pageIndex"></param>
        /// <param name="size"></param>
        /// <param name="isDescending"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order, int pageIndex, int size, bool isDescending)
        {
            var query = _mongoCollection.Find(filter).Skip(pageIndex * size).Limit(size);
            return (isDescending ? query.SortByDescending(order) : query.SortBy(order)).ToList();
        }

        /// <summary>
        ///  Find pageable record by filter and order
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="order"></param>
        /// <param name="pageIndex"></param>
        /// <param name="size"></param>
        /// <param name="isDescending"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order, int pageIndex, int size, bool isDescending)
        {
            var query = _mongoCollection.Find(filter).Skip(pageIndex * size).Limit(size);
            return await (isDescending ? query.SortByDescending(order) : query.SortBy(order)).ToListAsync();
        }

        #endregion

        #region FindAll Operations

        /// <summary>
        /// Find all record
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAll()
        {
            return _mongoCollection.Find(Builders<TEntity>.Filter.Empty).ToList();
        }

        /// <summary>
        /// Find all record
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> FindAllAsync()
        {
            return await _mongoCollection.Find(Builders<TEntity>.Filter.Empty).ToListAsync();
        }

        /// <summary>
        /// Find all pageable record
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAll(int pageIndex, int size)
        {
            return FindAll(i => i.Id, pageIndex, size);
        }

        /// <summary>
        /// Find all pageable record
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> FindAllAsync(int pageIndex, int size)
        {
            return await FindAllAsync(i => i.Id, pageIndex, size);
        }

        /// <summary>
        /// Find all pageable records and order records
        /// </summary>
        /// <param name="order"></param>
        /// <param name="pageIndex"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, object>> order, int pageIndex, int size)
        {
            return FindAll(order, pageIndex, size, true);
        }

        /// <summary>
        /// Find all pageable records and order records
        /// </summary>
        /// <param name="order"></param>
        /// <param name="pageIndex"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, object>> order, int? pageIndex = 0, int? size = 10)
        {
            return await FindAllAsync(order, pageIndex.Value, size.Value, true);
        }

        /// <summary>
        /// Find all pageable records and order records
        /// </summary>
        /// <param name="order"></param>
        /// <param name="pageIndex"></param>
        /// <param name="size"></param>
        /// <param name="isDescending"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, object>> order, int pageIndex, int size, bool isDescending)
        {
            var query = _mongoCollection.Find(_ => true).Skip(pageIndex * size).Limit(size);
            return (isDescending ? query.SortByDescending(order) : query.SortBy(order)).ToList();
        }

        /// <summary>
        /// Find all pageable records and order records
        /// </summary>
        /// <param name="order"></param>
        /// <param name="pageIndex"></param>
        /// <param name="size"></param>
        /// <param name="isDescending"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, object>> order, int pageIndex, int size, bool isDescending)
        {
            var query = _mongoCollection.Find(_ => true).Skip(pageIndex * size).Limit(size);
            return await (isDescending ? query.SortByDescending(order) : query.SortBy(order)).ToListAsync();
        }

        #endregion

        #region First Operations

        /// <summary>
        /// Get first record
        /// </summary>
        /// <returns></returns>
        public TEntity First()
        {
            return FindAll(i => i.Id, 0, 1, false).FirstOrDefault();
        }

        /// <summary>
        /// Get first record
        /// </summary>
        /// <returns></returns>
        public async Task<TEntity> FirstAsync()
        {
            var query = _mongoCollection.Find(_ => true).Skip(0 * 1).Limit(1);
            return await query.SortBy(i => i.Id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get first record by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public TEntity First(Expression<Func<TEntity, bool>> filter)
        {
            return First(filter, i => i.Id);
        }

        /// <summary>
        /// Get first record by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await FirstAsync(filter, i => i.Id);
        }

        /// <summary>
        /// Get first record by filter and order record
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public TEntity First(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order)
        {
            return First(filter, order, false);
        }

        /// <summary>
        /// Get first record by filter and order record
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order)
        {
            return await FirstAsync(filter, order, false);
        }

        /// <summary>
        /// Get first record by filter and order record
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="order"></param>
        /// <param name="isDescending"></param>
        /// <returns></returns>
        public TEntity First(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order, bool isDescending)
        {
            return Find(filter, order, 0, 1, isDescending).SingleOrDefault();
        }

        /// <summary>
        /// Get first record by filter and order record
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="order"></param>
        /// <param name="isDescending"></param>
        /// <returns></returns>
        public async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order, bool isDescending)
        {
            var query = _mongoCollection.Find(filter).Skip(0 * 1).Limit(1);
            return await (isDescending ? query.SortByDescending(order) : query.SortBy(order)).SingleOrDefaultAsync();
        }

        #endregion

        #region Last Operations

        /// <summary>
        /// Get Last Record
        /// </summary>
        /// <returns></returns>
        public TEntity Last()
        {
            return FindAll(i => i.Id, 0, 1, true).FirstOrDefault();
        }

        /// <summary>
        /// Get Last Record by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public TEntity Last(Expression<Func<TEntity, bool>> filter)
        {
            return Last(filter, i => i.Id);
        }

        /// <summary>
        /// Get Last Record by filter and order record
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public TEntity Last(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order)
        {
            return Last(filter, order, false);
        }

        /// <summary>
        /// Get Last Record by filter and order record
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="order"></param>
        /// <param name="isDescending"></param>
        /// <returns></returns>
        public TEntity Last(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order, bool isDescending)
        {
            return First(filter, order, !isDescending);
        }

        #endregion

        #region Update Operations

        /// <summary>
        /// Update Record
        /// </summary>
        /// <typeparam name="TField"></typeparam>
        /// <param name="entity"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Update<TField>(TEntity entity, Expression<Func<TEntity, TField>> field, TField value)
        {
            return Update(entity, Builders<TEntity>.Update.Set(field, value));
        }

        /// <summary>
        /// Update Record
        /// </summary>
        /// <typeparam name="TField"></typeparam>
        /// <param name="entity"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync<TField>(TEntity entity, Expression<Func<TEntity, TField>> field, TField value)
        {
            return await UpdateAsync(entity, Builders<TEntity>.Update.Set(field, value));
        }

        /// <summary>
        /// Update Record by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updates"></param>
        /// <returns></returns>
        public bool Update(string id, params UpdateDefinition<TEntity>[] updates)
        {
            return Update(Builders<TEntity>.Filter.Eq(i => i.Id, id), updates);
        }

        /// <summary>
        /// Update Record by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updates"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(string id, params UpdateDefinition<TEntity>[] updates)
        {
            return await UpdateAsync(Builders<TEntity>.Filter.Eq(i => i.Id, id), updates);
        }

        /// <summary>
        /// Update Record
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="updates"></param>
        /// <returns></returns>
        public bool Update(TEntity entity, params UpdateDefinition<TEntity>[] updates)
        {
            return Update(entity.Id, updates);
        }

        /// <summary>
        /// Update Record
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="updates"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(TEntity entity, params UpdateDefinition<TEntity>[] updates)
        {
            return await UpdateAsync(entity.Id, updates);
        }

        /// <summary>
        /// Update record
        /// </summary>
        /// <typeparam name="TField"></typeparam>
        /// <param name="filter"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Update<TField>(FilterDefinition<TEntity> filter, Expression<Func<TEntity, TField>> field, TField value)
        {
            return Update(filter, Builders<TEntity>.Update.Set(field, value));
        }

        /// <summary>
        /// Update record
        /// </summary>
        /// <typeparam name="TField"></typeparam>
        /// <param name="filter"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync<TField>(FilterDefinition<TEntity> filter, Expression<Func<TEntity, TField>> field, TField value)
        {
            return await UpdateAsync(filter, Builders<TEntity>.Update.Set(field, value));
        }

        /// <summary>
        /// Update record
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="updates"></param>
        /// <returns></returns>
        public bool Update(FilterDefinition<TEntity> filter, params UpdateDefinition<TEntity>[] updates)
        {
            var update = Builders<TEntity>.Update.Combine(updates).CurrentDate(i => i.ModifiedOn);
            return _mongoCollection.UpdateMany(filter, update.CurrentDate(i => i.ModifiedOn)).IsAcknowledged;
        }

        /// <summary>
        /// Update record
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="updates"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(FilterDefinition<TEntity> filter, params UpdateDefinition<TEntity>[] updates)
        {
            var update = Builders<TEntity>.Update.Combine(updates).CurrentDate(i => i.ModifiedOn);
            var res = await _mongoCollection.UpdateManyAsync(filter, update.CurrentDate(i => i.ModifiedOn));
            return res.IsAcknowledged;
        }

        /// <summary>
        /// Update record
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="updates"></param>
        /// <returns></returns>
        public bool Update(Expression<Func<TEntity, bool>> filter, params UpdateDefinition<TEntity>[] updates)
        {
            var update = Builders<TEntity>.Update.Combine(updates).CurrentDate(i => i.ModifiedOn);
            return _mongoCollection.UpdateMany(filter, update).IsAcknowledged;
        }

        /// <summary>
        /// Update record
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="updates"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(Expression<Func<TEntity, bool>> filter, params UpdateDefinition<TEntity>[] updates)
        {
            var update = Builders<TEntity>.Update.Combine(updates).CurrentDate(i => i.ModifiedOn);
            var res = await _mongoCollection.UpdateManyAsync(filter, update);
            return res.IsAcknowledged;
        }

        #endregion

        #region Any Operations

        /// <summary>
        /// Check Record by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public bool Any(Expression<Func<TEntity, bool>> filter)
        {
            return _mongoCollection.AsQueryable().Any(filter);
        }

        #endregion

        #endregion
    }
}
