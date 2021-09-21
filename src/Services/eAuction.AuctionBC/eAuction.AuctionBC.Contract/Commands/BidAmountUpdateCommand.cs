using System;
using MediatR;

namespace eAuction.AuctionBC.Contract.Commands
{
    public record BidAmountUpdateCommand(Guid CorrelationId, string BuyerId,  string AuctionItemId, double BidAmount);
    public record BidAmountUpdatedEvent(Guid CorrelationId, string BuyerId, string AuctionItemId) : INotification;
}
