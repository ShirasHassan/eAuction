using System;
namespace eAuction.Buyer.Contract.Queries
{
    public class GetBuyerId
    {
        public record ByEmail(Guid CorrelationId, string EmailId);

        public record Response(Guid CorrelationId, string BuyerId);
    }
    
}
