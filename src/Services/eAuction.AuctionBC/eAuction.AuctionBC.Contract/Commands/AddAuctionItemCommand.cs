

using System;
using MediatR;

namespace eAuction.AuctionBC.Contract.Commands
{
    public record AddAuctionItemCommand (Guid CorrelationId, string ItemId,
        string SellerId,
        string SellerName,
        string ItemName,
        string ShortDescription,
        string DetailedDescription,
        string Category,
        double StartingPrice,
        DateTime BidEndDate);
    public record AuctionItemAddedEvent(Guid CorrelationId,string ProductId): INotification;

    public record AddAuctionItemFailedEvent(Guid CorrelationId, string Message) : INotification;

}