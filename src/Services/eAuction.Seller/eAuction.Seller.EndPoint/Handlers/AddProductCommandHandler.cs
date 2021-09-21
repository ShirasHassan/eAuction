using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using eAuction.Seller.Contract.Commands;
using eAuction.Seller.Domain.SellerAggregate;
using MassTransit;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace eAuction.Seller.EndPoint.Handlers
{
    /// <summary>
    /// AddProductCommandHandler
    /// </summary>
    public class AddProductCommandHandler : IConsumer<AddProductCommand>
    {

        readonly ILogger<AddProductCommandHandler> _logger;
        private readonly ISellerRepository _sellerRepository;
        readonly IPublishEndpoint _endpoint;

        /// <summary>
        /// AddProductCommandHandler
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="sellerRepository"></param>
        /// <param name="endpoint"></param>
        public AddProductCommandHandler(ILogger<AddProductCommandHandler> logger, ISellerRepository sellerRepository,
            IPublishEndpoint endpoint)
        {
            _logger = logger;
            _sellerRepository = sellerRepository;
            _endpoint = endpoint;
        }

        /// <summary>
        /// Consumer/Handler
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<AddProductCommand> context)
        {
            Expression<Func<Domain.SellerAggregate.Seller, IList<Domain.SellerAggregate.Product>>> userNameExpression = x => x.Products;
            var field = new ExpressionFieldDefinition<Domain.SellerAggregate.Seller>(userNameExpression);
            await _sellerRepository.PushItemToArray(context.Message.SellerId, field, context.Message.Product);
            await _sellerRepository.UnitOfWork.SaveChangesAsync();
            _logger.LogInformation("Value: {Value}", context.Message);
            await _endpoint.Publish(new ProductAddedEvent(context.Message.CorrelationId, context.Message.Product.Id));
        }
    }
}
