using System;
using eAuction.BaseLibrary.Domain;

namespace eAuction.Seller.Domain.SellerAggregate
{
    public class Product : Entity
    {
        public string ProductName { get; private set; }
        public string ShortDescription { get; private set; }
        public string DetailedDescription { get; private set; }
        public string Category { get; private set; }
        public double StartingPrice { get; private set; }
        public DateTime BidEndDate { get; private set; }

        public Product(string id, string productName, string shortDescription, string detailedDescription, string category, double startingPrice, DateTime bidEndDate): this(productName,  shortDescription,  detailedDescription,  category,  startingPrice,  bidEndDate)
        {
            Id = id;
        }

        public Product(string productName, string shortDescription, string detailedDescription, string category, double startingPrice, DateTime bidEndDate) : this()
        {
            ProductName = productName;
            ShortDescription = shortDescription;
            DetailedDescription = detailedDescription;
            Category = category;
            StartingPrice = startingPrice;
            BidEndDate = bidEndDate;
           
        }

        public Product()
        {
            Id = Guid.NewGuid().ToString();
        }

    }
}
