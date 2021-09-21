using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eAuction.Seller.Message
{
   public class ProductInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    
    public class ListProductResponse
    {
        public Guid CorrelationId { get; set; }
        public List<Product> Products { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Exception Exception { get; set; }
    }
    
    public class ListProductRequest
    {
        public Guid CorrelationId { get; set; }
        public string EmailId { get; set; }
    }
}
