using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eAuction.Seller.Api.ProductEndpoints
{
    public class GetProductsResult
    {
        public Product Product { get; set; }
        public List<BidModel> Bids { get; set; }
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
        public string ProductName { get; set; }

        public string ShortDescription { get; set; }

        public string DetailedDescription { get; set; }

        public string Category { get; set; }

        public string StartingPrice { get; set; }
 
        public DateTime BidEndDate { get; set; }
    }
}
