using System;
using System.Threading.Tasks;
using eAuction.Buyer.Contract.Commands;
using eAuction.Buyer.Domain.BuyerAggregate;
using MassTransit;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace eAuction.Buyer.EndPoint.Handlers
{
    public class DeleteBuyerBidCommandHandler : IConsumer<DeleteBuyerBid.Command>
    {

        readonly ILogger<DeleteBuyerBidCommandHandler> _logger;
        private readonly IBuyerRepository _buyerRepository;
        readonly IPublishEndpoint _endpoint;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="BuyerRepository"></param>
        public DeleteBuyerBidCommandHandler(ILogger<DeleteBuyerBidCommandHandler> logger, IBuyerRepository buyerRepository,
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
        public async Task Consume(ConsumeContext<DeleteBuyerBid.Command> context)
        {
            try
            {
                var filter = Builders<Domain.BuyerAggregate.Buyer>.Filter.Where(buyer => buyer.Id == context.Message.BuyerId);
                var update = Builders<Domain.BuyerAggregate.Buyer>.Update.PullFilter(buyer => buyer.Bids,
                    Builders<AuctionItem>.Filter.Where(nm => nm.Id == context.Message.AuctionItemId));
               var result = await _buyerRepository.UpdateOneAsync(filter, update);
                await _buyerRepository.UnitOfWork.SaveChangesAsync();
                _logger.LogInformation("Value: {Value}", context.Message);
                await _endpoint.Publish(new DeleteBuyerBid.SuccessEvent(context.Message.CorrelationId, context.Message.BuyerId));
            }
            catch (Exception e)
            {
                await _endpoint.Publish(new DeleteBuyerBid.FailedEvent(context.Message.CorrelationId, e.Message));
            }

        }
    }
}

