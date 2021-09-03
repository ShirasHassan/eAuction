using eAuction.Seller.Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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

        public Product(string productName, string shortDescription, string detailedDescription, string category, double startingPrice, DateTime bidEndDate)
        {
            ProductName = productName;
            ShortDescription = shortDescription;
            DetailedDescription = detailedDescription;
            Category = category;
            StartingPrice = startingPrice;
            BidEndDate = bidEndDate;
        }

    }
}
