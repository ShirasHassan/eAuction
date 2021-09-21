using System;
using MediatR;

namespace eAuction.Seller.Contract.Commands
{
    public record DeleteProductCommand(Guid CorrelationId, string ProductId, string SellerId);
    public record ProductDeletedEvent(Guid CorrelationId,string ProductId): INotification;
}