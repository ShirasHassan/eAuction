using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eAuction.Seller.Message
{
    public class ProductAddedResponse
    {
        public Guid CorrelationId { get; set; }
        public string SellerId { get; set; }
        public string ProductId { get; set; }
    }
}