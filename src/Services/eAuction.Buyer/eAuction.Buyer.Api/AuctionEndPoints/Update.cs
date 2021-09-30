using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using eAuction.Buyer.Contract.Message;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace eAuction.Buyer.Api.AuctionEndPoints
{
    /// <summary>
    /// Create
    /// </summary>
    [Route("e-auction/api/v1/buyer")]
    public class Update : BaseAsyncEndpoint
       .WithRequest<UpdateAuctionRequest>
       .WithResponse<AuctionUpdatedResponse>
    {

        private readonly IPublishEndpoint _endpoint;
        private readonly IRequestClient<UpdateAuctionRequest> _requestClient;
        private readonly ILogger<Add> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="requestClient"></param>
        /// <param name="logger"></param>
        public Update(IPublishEndpoint endpoint, IRequestClient<UpdateAuctionRequest> requestClient, ILogger<Add> logger)
        {
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
        [HttpPost("/update-bid/{productId}/{buyerEmailld}/{newBidAmount}")]
        [SwaggerOperation(
            Summary = "place a new bid",
            Description = "add a new bid",
            OperationId = "Auction.Add",
            Tags = new[] { "AuctionEndpoints" })
        ]
        public override async Task<ActionResult<AuctionUpdatedResponse>> HandleAsync([FromRoute] UpdateAuctionRequest request, CancellationToken cancellationToken)
        {
            request.CorrelationId = Guid.NewGuid();
            var result = await _requestClient.GetResponse<AuctionUpdatedResponse>(request);
            return Ok(result);
        }
    }
}
