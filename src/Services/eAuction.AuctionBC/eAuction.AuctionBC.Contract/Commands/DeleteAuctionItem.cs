using System;
using MediatR;

namespace eAuction.AuctionBC.Contract.Commands
{
    public class DeleteAuctionItem
    {
        public record Command(Guid CorrelationId, string ProductId);
        public record SuccessEvent(Guid CorrelationId, string ProductId, string SellerId);
        public record FailedEvent(Guid CorrelationId, string Message) ;
    }
}