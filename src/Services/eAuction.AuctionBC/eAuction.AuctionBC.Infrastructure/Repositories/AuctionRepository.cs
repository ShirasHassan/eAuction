using eAuction.AuctionBC.Domain.AuctionItemAggregate;
using eAuction.BaseLibrary.Domain;
using eAuction.BaseLibrary.Repositories;


namespace eAuction.AuctionBC.Infrastructure.Repositories
{
    public class AuctionRepository : BaseRepository<AuctionItem>, IAuctionRepository
    {
        public AuctionRepository(IMongoContext context) : base(context)
        {
        }
    }
}
