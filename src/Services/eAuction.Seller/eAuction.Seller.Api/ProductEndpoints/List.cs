using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eAuction.Seller.Api.ProductEndpoints
{
    public class List : BaseAsyncEndpoint
        .WithRequest<string>
        .WithResponse<IList<ProductInfo>>
    {


        public List()
        {
        }

        [HttpGet("/e-auction/api/v1/seller/{id}/products")]
        [SwaggerOperation(
            Summary = "List all Products",
            Description = "List all Products",
            OperationId = "Product.List",
            Tags = new[] { "ProductEndpoints" })
        ]
        public override async Task<ActionResult<IList<ProductInfo>>> HandleAsync(string id,
            CancellationToken cancellationToken = default)
        {

            return Ok("");
        }
    }
}
