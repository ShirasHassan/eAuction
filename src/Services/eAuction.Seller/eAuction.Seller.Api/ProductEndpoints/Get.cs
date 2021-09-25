using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using eAuction.AuctionBC.Contract.Queries;
using eAuction.Seller.Message;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace eAuction.Seller.Api.ProductEndpoints.Get
{
    [Route("")]
    public class Get : BaseAsyncEndpoint
         .WithRequest<string>
         .WithResponse<AuctionDetails>
    {

        private readonly IPublishEndpoint _endpoint;
        private readonly IRequestClient<GetAuctionDetailsQuery> _requestClient;
        private readonly ILogger<Get> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="requestClient"></param>
        /// <param name="logger"></param>
        public Get(IPublishEndpoint endpoint, IRequestClient<GetAuctionDetailsQuery> requestClient, ILogger<Get> logger)
        {
            _endpoint = endpoint;
            _requestClient = requestClient;
            _logger = logger;
        }

        [HttpGet("/show-bids/{id}")]
        [SwaggerOperation(
            Summary = "Get a specific Product",
            Description = "Get a specific Product",
            OperationId = "Product.Get",
            Tags = new[] { "ProductEndpoints" })
        ]
        public override async Task<ActionResult<AuctionDetails>> HandleAsync([FromRoute]string id, CancellationToken cancellationToken)
        {
            var request = new GetAuctionDetailsQuery( Guid.NewGuid(),  id );
            var result = await _requestClient.GetResponse<AuctionDetails>(request);
            return Ok(result);
        }
    }
}

