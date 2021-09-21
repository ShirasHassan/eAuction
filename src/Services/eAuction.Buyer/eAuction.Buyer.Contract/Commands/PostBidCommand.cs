using System;
using MediatR;

namespace eAuction.Buyer.Contract.Commands
{
    public record PostBidCommand(Guid CorrelationId, Buyer.Domain.BuyerAggregate.Buyer Buyer, string ProductId, string BidAmount);
    public record BidPostedtEvent(Guid CorrelationId, string ProductId) : INotification;
}
