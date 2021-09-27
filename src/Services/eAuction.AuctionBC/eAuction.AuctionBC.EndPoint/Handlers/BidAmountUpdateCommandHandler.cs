using System;
using System.Threading.Tasks;
using eAuction.AuctionBC.Contract.Commands;
using eAuction.AuctionBC.Domain.AuctionItemAggregate;
using eAuction.BaseLibrary.Domain;
using MassTransit;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace eAuction.AuctionBC.EndPoint.Handlers
{
    public class BidAmountUpdateCommandHandler : IConsumer<UpdateAuctionBidAmount.Command>
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
        public async Task Consume(ConsumeContext<UpdateAuctionBidAmount.Command> context)
        {
            try
            {
                var auctionItemFilter = Builders<AuctionItem>.Filter.And(Builders<AuctionItem>.Filter.Eq(c => c.Id, context.Message.ProductId),
                    Builders<AuctionItem>.Filter.Eq(c => c.Status.Name, EntityStatus.Active.Name));

                var auctionItemBidEndDate = _auctionRepository.Find(auctionItemFilter)?.Project(x => x.BidEndDate)?.FirstOrDefault();
                _logger.LogInformation("Value: {Value}", context.Message);
                if (auctionItemBidEndDate == null || auctionItemBidEndDate == DateTime.MinValue)
                {
                    await _endpoint.Publish(new UpdateAuctionBidAmount.FailedEvent(context.Message.CorrelationId, "Unable to find active auction item"));
                    return;
                }
                if (auctionItemBidEndDate < DateTime.Now)
                {
                    await _endpoint.Publish(new UpdateAuctionBidAmount.FailedEvent(context.Message.CorrelationId, "Unable to update bid after auction end date"));
                    return;
                }

                var filter = Builders<AuctionItem>.Filter.And(
                    auctionItemFilter,
                    Builders<AuctionItem>.Filter.ElemMatch(i => i.Bids, u => u.Id == context.Message.BuyerId));

                var update = Builders<AuctionItem>.Update
                    .Set(l => l.Bids[-1].BidAmount, context.Message.BidAmount)
                    .Set(l => l.Bids[-1].LastUpdatedTime, DateTime.Now)
                    .Set(l => l.LastUpdatedTime, DateTime.Now);
                var result =  await _auctionRepository.UpdateOneAsync(filter, update);
                await _auctionRepository.UnitOfWork.SaveChangesAsync();
                if (result.ModifiedCount == 1) {
                    await _endpoint.Publish(new UpdateAuctionBidAmount.SuccessEvent(context.Message.CorrelationId, context.Message.BuyerId, context.Message.ProductId));
                }
                else
                {
                    await _endpoint.Publish(new UpdateAuctionBidAmount.FailedEvent(context.Message.CorrelationId, $"Unable to find auction bid"));   
                }
            }
            catch (Exception e) {
                await _endpoint.Publish(new UpdateAuctionBidAmount.FailedEvent(context.Message.CorrelationId, e.Message));
            }


        }

    }
}