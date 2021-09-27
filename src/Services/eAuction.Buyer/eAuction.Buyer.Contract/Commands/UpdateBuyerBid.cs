using System;
using MediatR;

namespace eAuction.Buyer.Contract.Commands
{
    public class UpdateBuyerBid
    {
        public record Command(Guid CorrelationId, string BuyerId, string AuctionItemId, double BidAmount);
        public record SuccessEvent(Guid CorrelationId, string BuyerId, string AuctionItemId);
        public record FailedEvent(Guid CorrelationId, string Message);
    }
}
