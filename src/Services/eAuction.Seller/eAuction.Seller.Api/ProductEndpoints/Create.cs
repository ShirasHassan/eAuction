using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using eAuction.Seller.Contract.Commands;
using MassTransit;
using Microsoft.Extensions.Logging;
using eAuction.Seller.Contract.Messages;
using Response = eAuction.Seller.Contract.Messages.Response;

namespace eAuction.Seller.Api.ProductEndpoints
{
    public class Create : BaseAsyncEndpoint
       .WithRequest<AddProductCommand>
       .WithResponse<AddProductResult>
    {

        private readonly IPublishEndpoint _endpoint;
        private readonly IRequestClient<AddSellerProduct> _requestClient;
        private readonly ILogger<Create> _logger;
        public Create(IPublishEndpoint endpoint,IRequestClient<AddSellerProduct> requestClient,ILogger<Create> logger) {
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
            var seller = new Domain.SellerAggregate.Seller("8b6ae036-0045-4957-a0e5-5637c3278237", 
                request.Seller.FirstName,
                request.Seller.LastName,
                request.Seller.Address,
                request.Seller.City, request.Seller.State, request.Seller.Pin, request.Seller.Phone,
                request.Seller.Email);

            seller.VerifyAndAddProduct(request.Product.ProductName, request.Product.ShortDescription,
                request.Product.DetailedDescription, request.Product.Category,double.Parse(request.Product.StartingPrice),
                request.Product.BidEndDate);
            await _endpoint.Publish<AddSellerProduct>(new {Id = Guid.NewGuid(), Seller = seller}, cancellationToken);
            var response = await _requestClient.GetResponse<Response>(new { Id = Guid.NewGuid(), Seller = seller });
            _logger.LogInformation("Value: {Value}", response.Message.Message);
            return Ok(response.Message.Message);
        }
    }
}
