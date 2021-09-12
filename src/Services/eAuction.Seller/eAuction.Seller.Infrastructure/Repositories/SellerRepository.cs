using eAuction.BaseLibrary.Domain;
using eAuction.BaseLibrary.Repositories;
using eAuction.Seller.Domain.SellerAggregate;

namespace eAuction.Seller.Infrastructure.Repositories
{
    public class SellerRepository : BaseRepository<Domain.SellerAggregate.Seller>, ISellerRepository
    {
        public SellerRepository(IMongoContext context) : base(context)
        {
        }
    }
}
