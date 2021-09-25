using System;
using System.Threading.Tasks;
using eAuction.Buyer.Contract.Commands;
using eAuction.Buyer.Domain.BuyerAggregate;
using MassTransit;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace eAuction.Buyer.EndPoint.Handlers
{
    public class UpdateBidRequestCommandHandler : IConsumer<UpdateBidRequestCommand>
    {

        readonly ILogger<UpdateBidRequestCommandHandler> _logger;
        private readonly IBuyerRepository _BuyerRepository;
        readonly IPublishEndpoint _endpoint;

        /// <summary>
        /// AddProductCommandHandler
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="BuyerRepository"></param>
        /// <param name="endpoint"></param>
        public UpdateBidRequestCommandHandler(ILogger<UpdateBidRequestCommandHandler> logger, IBuyerRepository BuyerRepository,
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
        public async Task Consume(ConsumeContext<UpdateBidRequestCommand> context)
        {
            //  var update = Builders<Domain.BuyerAggregate.Buyer>.Update.PullFilter(Buyer => Buyer.Products, Builders<Product>.Filter.Where(product => product.Id == context.Message.ProductId));
            var filter = Builders<Domain.BuyerAggregate.Buyer>.Filter.And(
                Builders<Domain.BuyerAggregate.Buyer>.Filter.Where(Buyer => Buyer.Id == context.Message.BuyerId),
            Builders<Domain.BuyerAggregate.Buyer>.Filter.ElemMatch(i => i.Bids, u => u.Id == context.Message.AuctionItemId));

            var update = Builders<Domain.BuyerAggregate.Buyer>.Update
                .Set(l => l.Bids[-1].BidAmount, context.Message.BidAmount)
                .Set(l => l.Bids[-1].LastUpdatedTime, DateTime.Now)
                .Set(l => l.LastUpdatedTime, DateTime.Now);
            await _BuyerRepository.UpdateOneAsync(filter, update);
            await _BuyerRepository.UnitOfWork.SaveChangesAsync();
            _logger.LogInformation("Value: {Value}", context.Message);
            await _endpoint.Publish(new UpdatedBidEvent(context.Message.CorrelationId, context.Message.BuyerId, context.Message.AuctionItemId));
        }


    }
}

