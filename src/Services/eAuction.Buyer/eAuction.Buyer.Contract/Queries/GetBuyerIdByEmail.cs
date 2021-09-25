using System;
namespace eAuction.Buyer.Contract.Queries
{
    public record GetBuyerIdByEmail(Guid CorrelationId, string EmailId);

    public record GetBuyerIdResponse(Guid CorrelationId, string BuyerId);
}
