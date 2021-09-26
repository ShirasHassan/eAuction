using System;
using MediatR;

namespace eAuction.Buyer.Contract.Commands
{
    public class CreateBuyerComand
    {
        public record CreateBuyerCommand(Guid CorrelationId, Domain.BuyerAggregate.Buyer Buyer);
        public record BuyerCreatedEvent(Guid CorrelationId, string BuyerId) : INotification;
        public record BuyerCreationFailed(Guid CorrelationId, string Message);
    }
}
