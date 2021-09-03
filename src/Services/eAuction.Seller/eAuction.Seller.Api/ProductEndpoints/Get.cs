using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eAuction.Seller.Api.ProductEndpoints.Get
{
    public class Get : BaseAsyncEndpoint
         .WithRequest<string>
         .WithResponse<GetProductsResult>
    {


        [HttpGet("/e-auction/api/v1/seller/show-bids/{id}")]
        [SwaggerOperation(
            Summary = "Get a specific Product",
            Description = "Get a specific Product",
            OperationId = "Product.Get",
            Tags = new[] { "ProductEndpoints" })
        ]
        public override async Task<ActionResult<GetProductsResult>> HandleAsync(string id, CancellationToken cancellationToken)
        {

            return Ok();
        }
    }
}

