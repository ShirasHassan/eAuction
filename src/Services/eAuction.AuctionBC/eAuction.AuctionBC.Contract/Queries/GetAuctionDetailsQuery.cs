using System;
namespace eAuction.AuctionBC.Contract.Queries
{
    public record GetAuctionDetailsQuery (Guid CorrelationId, string AuctionItemId);
    public record AuctionDetails(Guid CorrelationId, AuctionItemModel AuctionItem);
    public class BidModel
    {
        public double BidId { get; set; }

        public double BidAmount { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }
    }
    public class AuctionItemModel
    {
        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public string ShortDescription { get; set; }

        public string DetailedDescription { get; set; }

        public string Category { get; set; }

        public string StartingPrice { get; set; }

        public DateTime BidEndDate { get; set; }
    }
}
