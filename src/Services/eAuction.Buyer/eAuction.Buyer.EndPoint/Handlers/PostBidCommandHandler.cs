using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using eAuction.Buyer.Contract.Commands;
using eAuction.Buyer.Domain.BuyerAggregate;
using MassTransit;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace eAuction.Buyer.EndPoint.Handlers
{
    public class PostBidCommandHandler : IConsumer<PostBidCommand>
    {

        readonly ILogger<PostBidCommandHandler> _logger;
        private readonly IBuyerRepository _BuyerRepository;
        readonly IPublishEndpoint _endpoint;

        /// <summary>
        /// PostBidCommandHandler
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="BuyerRepository"></param>
        /// <param name="endpoint"></param>
        public PostBidCommandHandler(ILogger<PostBidCommandHandler> logger, IBuyerRepository BuyerRepository,
            IPublishEndpoint endpoint)
        {
            _logger = logger;
            _BuyerRepository = BuyerRepository;
            _endpoint = endpoint;
        }

        /// <summary>
        /// Consumer/Handler
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<PostBidCommand> context)
        {
            Expression<Func<Domain.BuyerAggregate.Buyer, IList<Domain.BuyerAggregate.AuctionItem>>> expression = x => x.Bids;
            var field = new ExpressionFieldDefinition<Domain.BuyerAggregate.Buyer>(expression);
            await _BuyerRepository.PushItemToArray(context.Message.BuyerId, field,
                new Domain.BuyerAggregate.AuctionItem(context.Message.AuctionItemId, double.Parse(context.Message.BidAmount)));
            await _BuyerRepository.UnitOfWork.SaveChangesAsync();
            _logger.LogInformation("Value: {Value}", context.Message);
            await _endpoint.Publish(new BidPostedEvent(context.Message.CorrelationId, context.Message.AuctionItemId));
        }
    }
}

