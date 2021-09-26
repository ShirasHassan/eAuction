using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using eAuction.AuctionBC.Contract.Commands;
using eAuction.AuctionBC.Domain.AuctionItemAggregate;
using eAuction.BaseLibrary.Domain;
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
                var auctionItemFilter = Builders<AuctionItem>.Filter.And(Builders<AuctionItem>.Filter.Eq(c => c.Id, context.Message.AuctionItemId),
                    Builders<AuctionItem>.Filter.Eq(c => c.Status.Name, EntityStatus.Active.Name));
                var auctionItemBidEndDate = _auctionRepository.Find(auctionItemFilter)?.Project(x => x.BidEndDate)?.FirstOrDefault();
                _logger.LogInformation("Value: {Value}", context.Message);

                if(auctionItemBidEndDate == null) {
                    await _endpoint.Publish(new BidFailedEvent(context.Message.CorrelationId, "Unable to find active auction item"));
                    return;
                }
                if(auctionItemBidEndDate < DateTime.Now) {
                    await _endpoint.Publish(new BidFailedEvent(context.Message.CorrelationId, "Unable to place bid after auction end date"));
                    return;
                }

                var filter = Builders<AuctionItem>.Filter.And(
                    auctionItemFilter,
                    Builders<AuctionItem>.Filter.Nin("Bids._id",  new[] { context.Message.BuyerId }));

                var bid = new Bid(context.Message.BuyerId, context.Message.BuyerName, context.Message.Phone, context.Message.Email, context.Message.BidAmount);

                var update = Builders<AuctionItem>.Update
                   .AddToSet(i => i.Bids, bid);

                var result = await _auctionRepository.UpdateOneAsync(filter, update);
                await _auctionRepository.UnitOfWork.SaveChangesAsync();
                if(result.ModifiedCount != 1)  {
                    await _endpoint.Publish(new BidFailedEvent(context.Message.CorrelationId, "Unable to post new bid, Please update existing posted bid."));
                }
                else
                {
                    await _endpoint.Publish(new BidPostedEvent(context.Message.CorrelationId, context.Message.BuyerId, context.Message.AuctionItemId));
                }
                _logger.LogInformation("Value: {Value}", context.Message);
                
            }
            catch (Exception e)
            {
                await _endpoint.Publish(new BidFailedEvent(context.Message.CorrelationId, e.Message));
            }
        }
    }
}
