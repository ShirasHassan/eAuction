using System;
using MediatR;

namespace eAuction.AuctionBC.Contract.Commands
{
    public record DeleteAuctionItemCommand(Guid CorrelationId, string ItemId);
    public record AuctionItemDeletedEvent(Guid CorrelationId, string ItemId,string SellerId): INotification;
    public record CommandFailedEvent(Guid CorrelationId, string message): INotification;

}