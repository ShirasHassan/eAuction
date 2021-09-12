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
    /// <summary>
    /// Create
    /// </summary>
    [Route("")]
    public class Create : BaseAsyncEndpoint
       .WithRequest<AddProductRequest>
       .WithResponse<ProductAddedResponse>
    {

        private readonly IPublishEndpoint _endpoint;
        private readonly IRequestClient<AddProductRequest> _requestClient;
        private readonly ILogger<Create> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="requestClient"></param>
        /// <param name="logger"></param>
        public Create(IPublishEndpoint endpoint,IRequestClient<AddProductRequest> requestClient,ILogger<Create> logger) {
            _endpoint = endpoint;
            _requestClient = requestClient;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new Product
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("/add-product")]
        [SwaggerOperation(
            Summary = "Creates a new Product",
            Description = "Creates a new Product",
            OperationId = "Product.Create",
            Tags = new[] { "ProductEndpoints" })
        ]
        public override async Task<ActionResult<ProductAddedResponse>> HandleAsync([FromBody] AddProductRequest request, CancellationToken cancellationToken)
        {
            request.CorrelationId = Guid.NewGuid();
            var result = await _requestClient.GetResponse<ProductAddedResponse>(request);
            return Ok(result);
        }
    }
}
