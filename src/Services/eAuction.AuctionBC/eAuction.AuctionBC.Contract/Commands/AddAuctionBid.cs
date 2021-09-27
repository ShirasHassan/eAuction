using System;
using MediatR;

namespace eAuction.AuctionBC.Contract.Commands
{
    public class AddAuctionBid
    {
        public record Command(Guid CorrelationId, string BuyerId, string BuyerName, string Phone, string Email, string ProductId, double BidAmount);
        public record SuccessEvent(Guid CorrelationId, string BuyerId, string ProductId);
        public record FailedEvent(Guid CorrelationId, string Message);
    }
}
