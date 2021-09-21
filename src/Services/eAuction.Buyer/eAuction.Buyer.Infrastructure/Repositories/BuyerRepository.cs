using System;
using eAuction.BaseLibrary.Domain;
using eAuction.BaseLibrary.Repositories;
using eAuction.Buyer.Domain.BuyerAggregate;

namespace eAuction.Buyer.Infrastructure.Repositories
{
    public class BuyerRepository : BaseRepository<Domain.BuyerAggregate.Buyer>, IBuyerRepository
    {
        public BuyerRepository(IMongoContext context) : base(context)
        {
        }
    }
}