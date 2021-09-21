using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using eAuction.BaseLibrary.Domain;
using MongoDB.Driver;

namespace eAuction.BaseLibrary.Repositories
{

    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : IAggregateRoot
    {
        protected readonly IMongoContext Context;
        protected IMongoCollection<TEntity> DbSet;

        public IUnitOfWork UnitOfWork {
            get  {
                return Context;
            }
        }

    protected BaseRepository(IMongoContext context)
        {
            Context = context;
            DbSet = Context.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public virtual void Add(TEntity obj)
        {
            Context.AddCommand(() => DbSet.InsertOneAsync(obj));
        }

        public virtual async Task<TEntity> GetById(string id)
        {
            var data = await DbSet.FindAsync(Builders<TEntity>.Filter.Eq("_id", id));
            return data.SingleOrDefault();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            var all = await DbSet.FindAsync(Builders<TEntity>.Filter.Empty);
            return all.ToList();
        }

        public virtual void Update(TEntity obj)
        {
            Context.AddCommand(() => DbSet.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", obj.Id), obj));
        }

        public virtual void Remove(string id)
        {
            Context.AddCommand(() => DbSet.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", id)));
        }

        public void Dispose()
        {
            Context?.Dispose();
        }

        public IQueryable<TEntity> AsQueryable()
        {
            return DbSet.AsQueryable<TEntity>();
        }

        public virtual IEnumerable<TEntity> FilterBy(Expression<Func<TEntity, bool>> filterExpression)
        {
            return DbSet.Find(filterExpression).ToEnumerable();
        }

        public virtual IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<TEntity, bool>> filterExpression,
            Expression<Func<TEntity, TProjected>> projectionExpression)
        {
            return DbSet.Find(filterExpression).Project(projectionExpression).ToEnumerable();
        }

        public virtual TEntity FindOne(Expression<Func<TEntity, bool>> filterExpression)
        {
            return DbSet.Find(filterExpression).FirstOrDefault();
        }

        public virtual Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            return Task.Run(() => DbSet.Find(filterExpression).FirstOrDefaultAsync());
        }

        public virtual TEntity FindById(string id)
        {
            var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, id);
            return DbSet.Find(filter).SingleOrDefault();
        }

        public virtual Task<TEntity> FindByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, id);
                return DbSet.Find(filter).SingleOrDefaultAsync();
            });
        }

        public async Task<UpdateResult> UpdateOneAsync(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update)
        {
            return await DbSet.UpdateOneAsync(filter, update);
        }

        public virtual void InsertOne(TEntity document)
        {
            DbSet.InsertOne(document);
        }

        public virtual Task InsertOneAsync(TEntity document)
        {
            return Task.Run(() => DbSet.InsertOneAsync(document));
        }

        public void InsertMany(ICollection<TEntity> documents)
        {
            DbSet.InsertMany(documents);
        }


        public virtual async Task InsertManyAsync(ICollection<TEntity> documents)
        {
            await DbSet.InsertManyAsync(documents);
        }

        public void ReplaceOne(TEntity document)
        {
            var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, document.Id);
            DbSet.FindOneAndReplace(filter, document);
        }

        public virtual async Task ReplaceOneAsync(TEntity document)
        {
            var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, document.Id);
            await DbSet.FindOneAndReplaceAsync(filter, document);
        }

        public TEntity DeleteOne(Expression<Func<TEntity, bool>> filterExpression)
        {
           return  DbSet.FindOneAndDelete(filterExpression);
        }

        public Task DeleteOneAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            return Task.Run(() => DbSet.FindOneAndDeleteAsync(filterExpression));
        }

        public void DeleteById(string id)
        {
            var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, id);
            DbSet.FindOneAndDelete(filter);
        }

        public Task DeleteByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, id);
                DbSet.FindOneAndDeleteAsync(filter);
            });
        }

        public void DeleteMany(Expression<Func<TEntity, bool>> filterExpression)
        {
            DbSet.DeleteMany(filterExpression);
        }

        public Task DeleteManyAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            return Task.Run(() => DbSet.DeleteManyAsync(filterExpression));
        }

        public async Task PushItemToArray<T>(string id, FieldDefinition<TEntity> fieldDefinition, T item )
        {
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id);
            var update = Builders<TEntity>.Update
                    .Push(fieldDefinition, item);
            await DbSet.FindOneAndUpdateAsync(filter, update);
        }

    }
}