using System;
using MediatR;

namespace eAuction.AuctionBC.Contract.Commands
{
    public record BidCommand(Guid CorrelationId,string BuyerId, string BuyerName , string Phone , string Email , string AuctionItemId, double BidAmount);
    public record BidPostedEvent(Guid CorrelationId, string BuyerId, string AuctionItemId) : INotification;
}
