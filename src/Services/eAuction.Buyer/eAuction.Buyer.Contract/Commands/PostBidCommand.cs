using System;
using MediatR;

namespace eAuction.Buyer.Contract.Commands
{
    public record PostBidCommand(Guid CorrelationId, string BuyerId, string AuctionItemId, string BidAmount);
    public record BidPostedEvent(Guid CorrelationId, string AuctionItemId) : INotification;
}
