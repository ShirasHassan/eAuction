using System;
namespace eAuction.Buyer.Contract.Commands
{
    public class DeleteBuyerBid
    {
        public record Command(Guid CorrelationId, string BuyerId, string AuctionItemId);
        public record SuccessEvent(Guid CorrelationId, string BuyerId);
        public record FailedEvent(Guid CorrelationId, string Message);
    }
}
