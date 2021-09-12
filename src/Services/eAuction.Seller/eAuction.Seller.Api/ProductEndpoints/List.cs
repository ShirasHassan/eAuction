using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using eAuction.Seller.Message;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace eAuction.Seller.Api.ProductEndpoints
{
    [Route("")]
    public class List : BaseAsyncEndpoint
        .WithRequest<string>
        .WithResponse<IList<ProductInfo>>
    {


        public List()
        {
        }

        [HttpGet("/{id}/products")]
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
