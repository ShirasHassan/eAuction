using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eAuction.Seller.Message
{
    public class GetSellerProductsRequest
    {
        public Guid CorrelationId { get; set; }
        public string SellerEmailId { get; set; }
    }
    
}
