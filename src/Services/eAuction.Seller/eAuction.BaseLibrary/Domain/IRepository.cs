using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace eAuction.BaseLibrary.Domain
{
    public interface IRepository<TDocument> where TDocument : IAggregateRoot
    {
        /// <summary>
        /// 
        /// </summary>
        IUnitOfWork UnitOfWork { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        void Add(TDocument obj);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TDocument> GetById(string id);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TDocument>> GetAll();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        void Update(TDocument obj);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        void Remove(string id);
        /// <summary>
        /// 
        /// </summary>
        void Dispose();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IQueryable<TDocument> AsQueryable();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterExpression"></param>
        /// <returns></returns>
        IEnumerable<TDocument> FilterBy(Expression<Func<TDocument, bool>> filterExpression);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TProjected"></typeparam>
        /// <param name="filterExpression"></param>
        /// <param name="projectionExpression"></param>
        /// <returns></returns>
        IEnumerable<TProjected> FilterBy<TProjected>(Expression<Func<TDocument, bool>> filterExpression,Expression<Func<TDocument, TProjected>> projectionExpression);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterExpression"></param>
        /// <returns></returns>
        TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterExpression"></param>
        /// <returns></returns>
        Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="fieldDefinition"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        Task PushItemToArray<T>(string id, FieldDefinition<TDocument> fieldDefinition,T item);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TDocument FindById(string id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TDocument> FindByIdAsync(string id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        void InsertOne(TDocument document);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        Task InsertOneAsync(TDocument document);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="documents"></param>
        void InsertMany(ICollection<TDocument> documents);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="documents"></param>
        /// <returns></returns>
        Task InsertManyAsync(ICollection<TDocument> documents);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        void ReplaceOne(TDocument document);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        Task ReplaceOneAsync(TDocument document);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterExpression"></param>
        TDocument DeleteOne(Expression<Func<TDocument, bool>> filterExpression);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterExpression"></param>
        /// <returns></returns>
        Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        void DeleteById(string id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteByIdAsync(string id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterExpression"></param>
        void DeleteMany(Expression<Func<TDocument, bool>> filterExpression);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterExpression"></param>
        /// <returns></returns>
        Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        Task<UpdateResult> UpdateOneAsync(FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> update);
}
}
