using System;
using MediatR;

namespace eAuction.Buyer.Contract.Commands
{
    public record UpdateBidRequestCommand(Guid CorrelationId, string BuyerId, string AuctionItemId, double BidAmount);
    public record UpdatedBidEvent(Guid CorrelationId, string BuyerId, string AuctionItemId) : INotification;
    public record BuyerBidUpdatingFailed(Guid CorrelationId, string Message);
}
