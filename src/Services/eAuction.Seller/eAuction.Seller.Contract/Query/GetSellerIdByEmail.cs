using System;
using MassTransit;

namespace eAuction.Seller.Contract.Query
{
    public record GetSellerIdByEmail(Guid CorrelationId, string EmailId);

    public record GetSellerIdResponse(Guid CorrelationId, string SellerId);

}
