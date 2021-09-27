using System;
using MassTransit;

namespace eAuction.Seller.Contract.Query
{
    public class GetSellerId
    {
        public record ByEmail(Guid CorrelationId, string EmailId);

        public record Response(Guid CorrelationId, string SellerId);
    }
}
