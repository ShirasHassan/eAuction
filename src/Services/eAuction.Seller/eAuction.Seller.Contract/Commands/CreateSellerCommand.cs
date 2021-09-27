using System;
using MediatR;

namespace eAuction.Seller.Contract.Commands
{
    public class CreateSeller
    {
        public record Command(Guid CorrelationId, Domain.SellerAggregate.Seller Seller);
        public record SuccessEvent(Guid CorrelationId, string SellerId);
        public record FailedEvent(Guid CorrelationId, string Message);
    }

}
