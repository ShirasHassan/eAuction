using System;
using MediatR;

namespace eAuction.Seller.Contract.Commands
{
    public class DeleteSellerProduct
    {
        public record Command(Guid CorrelationId, string ProductId, string SellerId);
        public record SuccessEvent(Guid CorrelationId, string ProductId);
        public record FailedEvent(Guid CorrelationId, string Message);
    }
   
}