﻿using eAuction.Seller.Domain.Seedwork;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eAuction.Seller.Infrastructure.Repositories
{
    
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : IAggregateRoot
    {
        protected readonly IMongoContext Context;
        protected IMongoCollection<TEntity> DbSet;

        public IUnitOfWork UnitOfWork
        {
            get
            {
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
            Context.AddCommand(() => DbSet.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", obj.GetId()), obj));
        }

        public virtual void Remove(string id)
        {
            Context.AddCommand(() => DbSet.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", id)));
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}