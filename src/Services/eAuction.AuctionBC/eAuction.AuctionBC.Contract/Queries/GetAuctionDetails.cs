using System;
using System.Collections.Generic;

namespace eAuction.AuctionBC.Contract.Queries
{
    public class GetAuctionDetails
    {
        public record ByProductId(Guid CorrelationId, string ProductId);
        public record Response(Guid CorrelationId, AuctionItemModel AuctionItem);
        public class BidModel
        {
            public double BidId { get; set; }

            public double BidAmount { get; set; }

            public string BuyerName { get; set; }

            public string Email { get; set; }

            public string Mobile { get; set; }
        }
        public class AuctionItemModel
        {
            public string Id { get; set; }

            public string ProductName { get; set; }

            public string ShortDescription { get; set; }

            public string DetailedDescription { get; set; }

            public string Category { get; set; }

            public string StartingPrice { get; set; }

            public DateTime BidEndDate { get; set; }
            public List<BidModel> Bids { get; set; }
        }
    }

    
}
