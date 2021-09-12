using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using eAuction.Seller.Message;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace eAuction.Seller.Api.ProductEndpoints.Get
{
    [Route("")]
    public class Get : BaseAsyncEndpoint
         .WithRequest<string>
         .WithResponse<GetProductResponse>
    {


        [HttpGet("/show-bids/{id}")]
        [SwaggerOperation(
            Summary = "Get a specific Product",
            Description = "Get a specific Product",
            OperationId = "Product.Get",
            Tags = new[] { "ProductEndpoints" })
        ]
        public override async Task<ActionResult<GetProductResponse>> HandleAsync(string id, CancellationToken cancellationToken)
        {

            return Ok();
        }
    }
}

