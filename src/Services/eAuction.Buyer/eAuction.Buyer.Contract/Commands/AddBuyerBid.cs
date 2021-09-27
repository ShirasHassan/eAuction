using System;
using MediatR;

namespace eAuction.Buyer.Contract.Commands
{
    public class AddBuyerBid
    {
        public record Command(Guid CorrelationId, string BuyerId, string AuctionItemId, string BidAmount);
        public record SuccessEvent(Guid CorrelationId, string AuctionItemId);
        public record FailedEvent(Guid CorrelationId, string Message);
    }
}
