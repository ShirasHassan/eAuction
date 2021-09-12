using eAuction.BaseLibrary.Domain;

namespace eAuction.AuctionBC.Domain.AuctionItemAggregate
{
    public class AuctionItem : IAggregateRoot
    {
        public string Id { get; }
        public object GetId()
        {
            throw new System.NotImplementedException();
        }
    }
}