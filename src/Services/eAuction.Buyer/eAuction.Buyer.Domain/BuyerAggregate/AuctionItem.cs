using System;
using eAuction.BaseLibrary.Domain;

namespace eAuction.Buyer.Domain.BuyerAggregate
{
    public class AuctionItem :  Entity
    {
        public double BidAmount { get; private set; }

        public AuctionItem(string id,  double bidAmount) 
        {
            Id = id;
            BidAmount = bidAmount;
        }

        public AuctionItem()
        {
            Id = Guid.NewGuid().ToString();
        }

    
}
}
