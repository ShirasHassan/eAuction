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
    public class Delete : BaseAsyncEndpoint
       .WithRequest<string>
       .WithResponse<DeleteProductResult>
    {


        public Delete()
        {

        }

        [HttpDelete("/e-auction/api/v1/seller/delete/{id}")]
        [SwaggerOperation(
            Summary = "Deletes a Product",
            Description = "Deletes a Product",
            OperationId = "Product.Delete",
            Tags = new[] { "ProductEndpoints" })
        ]
        public override async Task<ActionResult<DeleteProductResult>> HandleAsync(string id, CancellationToken cancellationToken)
        {
            return Ok("ok");
        }
    }
}
