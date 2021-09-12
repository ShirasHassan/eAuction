using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using eAuction.Seller.Message;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace eAuction.Seller.Api.ProductEndpoints
{
    [Route("")]
    public class Delete : BaseAsyncEndpoint
       .WithRequest<string>
       .WithResponse<ProductDeletedResponse>
    {


        public Delete()
        {

        }

        /// <summary>
        /// Deletes a Product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("/delete/{id}")]
        [SwaggerOperation(
            Summary = "Deletes a Product",
            Description = "Deletes a Product",
            OperationId = "Product.Delete",
            Tags = new[] { "ProductEndpoints" })
        ]
        public override async Task<ActionResult<ProductDeletedResponse>> HandleAsync(string id, CancellationToken cancellationToken)
        {
            return Ok("ok");
        }
    }
}
