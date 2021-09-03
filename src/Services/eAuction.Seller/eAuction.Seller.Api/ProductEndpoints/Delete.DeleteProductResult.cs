using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eAuction.Seller.Api.ProductEndpoints
{
    public class DeleteProductResult
    {
        public bool IsSuccessFull { get; set; }
        public List<string> Exceptions { get; set; }
    }
}
