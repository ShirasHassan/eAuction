using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using eAuction.AuctionBC.Contract.Commands;
using eAuction.AuctionBC.Domain.AuctionItemAggregate;
using MassTransit;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace eAuction.AuctionBC.EndPoint.Handlers
{
    public class BidCommandHandler : IConsumer<BidCommand>
    {

        readonly ILogger<BidCommandHandler> _logger;
        private readonly IAuctionRepository _auctionRepository;
        readonly IPublishEndpoint _endpoint;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="auctionRepository"></param>
        public BidCommandHandler(ILogger<BidCommandHandler> logger, IAuctionRepository auctionRepository, IPublishEndpoint endpoint)
        {
            _logger = logger;
            _auctionRepository = auctionRepository;
            _endpoint = endpoint;
        }

        /// <summary>
        /// Consumer/Handler
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<BidCommand> context)
        {
            try
            {
                Expression<Func<AuctionItem, IList<Bid>>> expression = x => x.Bids;
                var field = new ExpressionFieldDefinition<AuctionItem>(expression);
                await _auctionRepository.PushItemToArray(context.Message.AuctionItemId, field, new Bid(context.Message.BuyerId,
                    context.Message.BuyerName, context.Message.Phone, context.Message.Email, context.Message.BidAmount));
                await _auctionRepository.UnitOfWork.SaveChangesAsync();
                _logger.LogInformation("Value: {Value}", context.Message);
                await _endpoint.Publish(new BidPostedEvent(context.Message.CorrelationId, context.Message.BuyerId,  context.Message.AuctionItemId));
            }
            catch (Exception e)
            {
                await _endpoint.Publish(new CommandFailedEvent(context.Message.CorrelationId, e.Message));
            }


        }
    }
}
