using System;
using System.Text.Json.Serialization;

namespace eAuction.Buyer.Contract.Message
{
	public class UpdateAuctionRequest
    {
        public Guid CorrelationId { get; set; }
        public string BuyerId { get; set; }
        public string ProductId { get; set; }
        public string BidAmount { get; set; }
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
