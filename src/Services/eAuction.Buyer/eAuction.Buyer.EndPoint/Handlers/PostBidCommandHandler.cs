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
    public class PostBidCommandHandler : IConsumer<AddBuyerBid.Command>
    {

        readonly ILogger<PostBidCommandHandler> _logger;
        private readonly IBuyerRepository _buyerRepository;
        readonly IPublishEndpoint _endpoint;

        /// <summary>
        /// PostBidCommandHandler
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="buyerRepository"></param>
        /// <param name="endpoint"></param>
        public PostBidCommandHandler(ILogger<PostBidCommandHandler> logger, IBuyerRepository buyerRepository,
            IPublishEndpoint endpoint)
        {
            _logger = logger;
            _buyerRepository = buyerRepository;
            _endpoint = endpoint;
        }

        /// <summary>
        /// Consumer/Handler
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<AddBuyerBid.Command> context)
        {
            try
            {
                var auctionItem = await _buyerRepository.FindOneAsync(x => x.Id == context.Message.BuyerId);
                                        
                _logger.LogInformation("Value: {Value}", context.Message);

                var filter = Builders<Domain.BuyerAggregate.Buyer>.Filter.And(
                    Builders<Domain.BuyerAggregate.Buyer>.Filter.Eq(c => c.Id, context.Message.BuyerId),
                    Builders<Domain.BuyerAggregate.Buyer>.Filter.Nin("Bids._id", new[] { context.Message.BuyerId }));

                var bidItem = new Domain.BuyerAggregate.AuctionItem(context.Message.AuctionItemId, double.Parse(context.Message.BidAmount));

                var update = Builders<Domain.BuyerAggregate.Buyer>.Update
                   .AddToSet(i => i.Bids, bidItem);

                var result = await _buyerRepository.UpdateOneAsync(filter, update);
                await _buyerRepository.UnitOfWork.SaveChangesAsync();

                _logger.LogInformation("Value: {Value}", context.Message);
                if (result.ModifiedCount == 1)   {
                    await _endpoint.Publish(new AddBuyerBid.SuccessEvent(context.Message.CorrelationId, context.Message.AuctionItemId));
                }
                else {
                    await _endpoint.Publish(new AddBuyerBid.FailedEvent(context.Message.CorrelationId, "Unable to post new entry, please update your existing post."));
                }
               
            }
            catch (Exception e)
            {
                await _endpoint.Publish(new AddBuyerBid.FailedEvent(context.Message.CorrelationId, e.Message));
            }
            
        }
    }
}

