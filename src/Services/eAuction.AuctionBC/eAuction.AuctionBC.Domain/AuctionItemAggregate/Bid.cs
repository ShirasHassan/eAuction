using eAuction.BaseLibrary.Domain;

namespace eAuction.AuctionBC.Domain.AuctionItemAggregate
{
    public class Bid : Entity
    {
        public string BuyerName { get; private set; }
        public string Phone { get; private set; }
        public string Email { get; private set; }
        public double BidAmount { get; private set; }

        public Bid(string id, string buyerName, string phone, string email, double bidAmount)
        {
            Id = id;
            BuyerName = buyerName;
            Phone = phone;
            Email = email;
            BidAmount = bidAmount;
        }
    }
}