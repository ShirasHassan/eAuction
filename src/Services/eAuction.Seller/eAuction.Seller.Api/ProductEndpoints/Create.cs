using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace eAuction.Seller.Api.ProductEndpoints
{
    public class Create : BaseAsyncEndpoint
       .WithRequest<AddProductCommand>
       .WithResponse<AddProductResult>
    {

        private readonly IPublishEndpoint _endpoint;
        private readonly IRequestClient<IValueEntered> _requestClient;
        private readonly ILogger<Create> _logger;
        public Create(IPublishEndpoint endpoint,IRequestClient<IValueEntered> requestClient,ILogger<Create> logger) {
            _endpoint = endpoint;
            _requestClient = requestClient;
            _logger = logger;
        }

        [HttpPost(AddProductCommand.Route)]
        [SwaggerOperation(
            Summary = "Creates a new Product",
            Description = "Creates a new Product",
            OperationId = "Product.Create",
            Tags = new[] { "ProductEndpoints" })
        ]
        public override async Task<ActionResult<AddProductResult>> HandleAsync([FromBody] AddProductCommand request, CancellationToken cancellationToken)
        {
             await _endpoint.Publish<IValueEntered>(new {Value = "product created"}, cancellationToken);
            var response = await _requestClient.GetResponse<IValueProcessed>(new {Value = "product 2 is created"});
            _logger.LogInformation("Value: {Value}", response.Message.Value);
            return Ok("ok");
        }
    }
}
