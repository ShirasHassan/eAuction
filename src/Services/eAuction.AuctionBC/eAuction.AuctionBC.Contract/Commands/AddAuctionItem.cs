

using System;
using MediatR;

namespace eAuction.AuctionBC.Contract.Commands
{
    public class AddAuctionItem
    {
        public record Command(Guid CorrelationId, string ProductId,  string SellerId, string SellerName, string ItemName,string ShortDescription, string DetailedDescription, string Category, double StartingPrice,  DateTime BidEndDate);
        public record SuccessEvent(Guid CorrelationId, string ProductId) ;
        public record FailedEvent(Guid CorrelationId, string Message);
    }
}