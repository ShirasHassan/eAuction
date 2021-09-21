using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eAuction.Seller.Message
{
    public class ProductDeletedRequest
    {
        public Guid CorrelationId { get; set; }
        public string ProductId { get; set; }
    }
    public class ProductDeletedResponse
    {
        public Guid CorrelationId { get; set; }
        public string SellerId { get; set; }
        public string ProductId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Exception Exception { get; set; }
    }
}
