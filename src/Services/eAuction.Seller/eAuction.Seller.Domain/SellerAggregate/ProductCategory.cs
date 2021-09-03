using eAuction.Seller.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace eAuction.Seller.Domain.SellerAggregate
{
    class ProductCategory
     : Enumeration
    {
        public static ProductCategory Painting = new (1, nameof(Painting));
        public static ProductCategory Sculptor = new (2, nameof(Sculptor));
        public static ProductCategory Ornament = new (3, nameof(Ornament));

        public ProductCategory(int id, string name)
            : base(id, name)
        {
        }
    }
}
