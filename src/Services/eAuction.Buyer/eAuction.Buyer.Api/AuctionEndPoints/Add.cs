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
    public class Add : BaseAsyncEndpoint
       .WithRequest<AddAuctionRequest>
       .WithResponse<AuctionAddedResponse>
    {

        private readonly IPublishEndpoint _endpoint;
        private readonly IRequestClient<AddAuctionRequest> _requestClient;
        private readonly ILogger<Add> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="requestClient"></param>
        /// <param name="logger"></param>
        public Add(IPublishEndpoint endpoint, IRequestClient<AddAuctionRequest> requestClient, ILogger<Add> logger)
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
        [HttpPost("/place-bid")]
        [SwaggerOperation(
            Summary = "place a new bid",
            Description = "add a new bid",
            OperationId = "Auction.Add",
            Tags = new[] { "AuctionEndpoints" })
        ]
        public override async Task<ActionResult<AuctionAddedResponse>> HandleAsync([FromBody] AddAuctionRequest request, CancellationToken cancellationToken)
        {
            request.CorrelationId = Guid.NewGuid();
            var result = await _requestClient.GetResponse<AuctionAddedResponse>(request);
            return Ok(result);
        }
    }
}
