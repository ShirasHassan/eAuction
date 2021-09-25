using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace eAuction.Buyer.Contract.Message
{
	public class UpdateAuctionRequest
    {
        public Guid CorrelationId { get; set; }
        [FromRoute(Name = "buyerEmailld")] public string BuyerEmailId { get; set; }
        [FromRoute(Name = "productId")] public string ProductId { get; set; }
        [FromRoute(Name = "newBidAmount")] public string BidAmount { get; set; }
    }

    public class AuctionUpdatedResponse
    {
        public Guid CorrelationId { get; set; }
        public string BuyerId { get; set; }
        public string ProductId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Exception Exception { get; set; }
    }
}
