using System;
using MediatR;

namespace eAuction.Seller.Contract.Commands
{
    public record AddProductCommand(Guid CorrelationId, Domain.SellerAggregate.Product Product, string SellerId);
    public record ProductAddedEvent(Guid CorrelationId,string ProductId): INotification;
}