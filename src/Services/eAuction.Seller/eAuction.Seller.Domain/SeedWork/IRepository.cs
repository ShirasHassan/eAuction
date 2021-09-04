using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eAuction.Seller.Domain.Seedwork
{
    public interface IRepository<T> where T : IAggregateRoot
    {
           IUnitOfWork UnitOfWork { get; }
           void Add(T obj);
            Task<T> GetById(string id);
            Task<IEnumerable<T>> GetAll();
            void Update(T obj);
            void Remove(string id);
            void Dispose();
        

    }
}
