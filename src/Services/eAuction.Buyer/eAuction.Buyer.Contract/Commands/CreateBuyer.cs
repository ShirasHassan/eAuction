using System;
using MediatR;

namespace eAuction.Buyer.Contract.Commands
{
    public class CreateBuyer
    {
        public record Command(Guid CorrelationId, Domain.BuyerAggregate.Buyer Buyer);
        public record SuccessEvent(Guid CorrelationId, string BuyerId);
        public record FailedEvent(Guid CorrelationId, string Message);
    }
}
