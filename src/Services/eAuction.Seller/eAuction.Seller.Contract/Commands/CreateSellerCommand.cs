using System;
using MediatR;

namespace eAuction.Seller.Contract.Commands
{
    public record CreateSellerCommand(Guid CorrelationId, Domain.SellerAggregate.Seller Seller);
    public record SellerCreatedEvent(Guid CorrelationId, string SellerId) : INotification;
}
