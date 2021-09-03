using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace eAuction.Seller.Domain.Seedwork
{
    public interface IMongoContext : IDisposable, IUnitOfWork
    {
        void AddCommand(Func<Task> func);
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
