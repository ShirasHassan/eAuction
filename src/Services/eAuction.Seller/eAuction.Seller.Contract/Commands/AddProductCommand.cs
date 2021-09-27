using System;
using MediatR;

namespace eAuction.Seller.Contract.Commands
{
    public class AddSellerProduct
    {
        public record Command(Guid CorrelationId, Domain.SellerAggregate.Product Product, string SellerId);
        public record SuccessEvent(Guid CorrelationId, string ProductId);
        public record FailedEvent(Guid CorrelationId, string Message);
    }
}