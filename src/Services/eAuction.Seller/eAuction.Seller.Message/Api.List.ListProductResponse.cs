using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
