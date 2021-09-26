using System;
using MediatR;

namespace eAuction.AuctionBC.Contract.Commands
{
    public record DeleteAuctionItemCommand(Guid CorrelationId, string AuctionItemId);
    public record AuctionItemDeletedEvent(Guid CorrelationId, string AuctionItemId, string SellerId): INotification;
    public record DeleteAuctionItemFailedEvent(Guid CorrelationId, string Message): INotification;

}