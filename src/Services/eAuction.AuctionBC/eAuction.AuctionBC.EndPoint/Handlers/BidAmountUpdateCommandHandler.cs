using System;
using System.Threading.Tasks;
using eAuction.AuctionBC.Contract.Commands;
using eAuction.AuctionBC.Domain.AuctionItemAggregate;
using MassTransit;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace eAuction.AuctionBC.EndPoint.Handlers
{
    public class BidAmountUpdateCommandHandler : IConsumer<BidAmountUpdateCommand>
    {

        readonly ILogger<BidAmountUpdateCommandHandler> _logger;
        private readonly IAuctionRepository _auctionRepository;
        readonly IPublishEndpoint _endpoint;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="auctionRepository"></param>
        public BidAmountUpdateCommandHandler(ILogger<BidAmountUpdateCommandHandler> logger, IAuctionRepository auctionRepository, IPublishEndpoint endpoint)
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
        public async Task Consume(ConsumeContext<BidAmountUpdateCommand> context)
        {
            try
            {
                var auctionItem = await _auctionRepository.FindOneAsync(x => x.Id == context.Message.AuctionItemId);
                _logger.LogInformation("Value: {Value}", context.Message);

                var filter = Builders<AuctionItem>.Filter.And(
                    Builders<AuctionItem>.Filter.Eq(c => c.Id, context.Message.AuctionItemId),
                    Builders<AuctionItem>.Filter.ElemMatch(i => i.Bids, u => u.Id == context.Message.BuyerId));

                var update = Builders<AuctionItem>.Update
                    .Set(l => l.Bids[-1].BidAmount, context.Message.BidAmount)
                    .Set(l => l.Bids[-1].LastUpdatedTime, DateTime.Now)
                    .Set(l => l.LastUpdatedTime, DateTime.Now);
                var result =  await _auctionRepository.UpdateOneAsync(filter, update);
                await _auctionRepository.UnitOfWork.SaveChangesAsync();
                if (result.ModifiedCount == 1) {
                    await _endpoint.Publish(new BidAmountUpdatedEvent(context.Message.CorrelationId, context.Message.BuyerId, auctionItem.Id));
                }
                else
                {
                    if (auctionItem != null)
                    {
                        await _endpoint.Publish(new CommandFailedEvent(context.Message.CorrelationId, $"Unable to find auction bid"));
                    }
                }
            }
            catch (Exception e)
            {
                await _endpoint.Publish(new CommandFailedEvent(context.Message.CorrelationId, e.Message));
            }


        }

    }
}