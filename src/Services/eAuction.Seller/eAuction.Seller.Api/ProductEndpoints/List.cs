using System.Collections.Generic;
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
    [Route("")]
    public class List : BaseAsyncEndpoint
        .WithRequest<string>
        .WithResponse<IList<ProductInfo>>
    {


        private readonly IPublishEndpoint _endpoint;
        private readonly IRequestClient<ListProductRequest> _requestClient;
        private readonly ILogger<List> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="requestClient"></param>
        /// <param name="logger"></param>
        public List(IPublishEndpoint endpoint, IRequestClient<ListProductRequest> requestClient, ILogger<List> logger)
        {
            _endpoint = endpoint;
            _requestClient = requestClient;
            _logger = logger;
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
