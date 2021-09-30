using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using eAuction.Seller.Message;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace eAuction.Seller.Api.ProductEndpoints
{
    [Route("e-auction/api/v1/seller")]
    public class Delete : BaseAsyncEndpoint
       .WithRequest<string>
       .WithResponse<ProductDeletedResponse>
    {

        private readonly IPublishEndpoint _endpoint;
        private readonly IRequestClient<ProductDeletedRequest> _requestClient;
        private readonly ILogger<Delete> _logger;

        public Delete(IPublishEndpoint endpoint, IRequestClient<ProductDeletedRequest> requestClient, ILogger<Delete> logger)
        {
            _endpoint = endpoint;
            _requestClient = requestClient;
            _logger = logger;
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
            var request = new ProductDeletedRequest() { CorrelationId = Guid.NewGuid(), ProductId = id };
            var result = await _requestClient.GetResponse<ProductDeletedResponse>(request);
            return Ok(result?.Message);
        }
    }
}
