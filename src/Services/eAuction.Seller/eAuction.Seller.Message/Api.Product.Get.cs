using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eAuction.Seller.Message
{
    public class GetProductRequest
    {
        public Guid CorrelationId { get; set; }
        public string ProductId { get; set; }
    }
    
    public class GetProductResponse
    {
        public Guid CorrelationId { get; set; }
        public Product Product { get; set; }
        public List<BidModel> Bids { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Exception Exception { get; set; }
    }

    public class BidModel
    {
        public double BidAmount { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }
    }

    public class Product
    {
        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public string ShortDescription { get; set; }

        public string DetailedDescription { get; set; }

        public string Category { get; set; }

        public string StartingPrice { get; set; }
 
        public DateTime BidEndDate { get; set; }
    }
}
