using System;
using System.Threading.Tasks;
using eAuction.BaseLibrary.Domain;
using eAuction.Seller.Contract.Commands;
using eAuction.Seller.Domain.SellerAggregate;
using MassTransit;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace eAuction.Seller.EndPoint.Handlers
{
    public class DeleteProductCommandHandler : IConsumer<DeleteProductCommand>
    {

        readonly ILogger<DeleteProductCommandHandler> _logger;
        private readonly ISellerRepository _sellerRepository;
        readonly IPublishEndpoint _endpoint;

        /// <summary>
        /// AddProductCommandHandler
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="sellerRepository"></param>
        /// <param name="endpoint"></param>
        public DeleteProductCommandHandler(ILogger<DeleteProductCommandHandler> logger, ISellerRepository sellerRepository,
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
        public async Task Consume(ConsumeContext<DeleteProductCommand> context)
        {
            //  var update = Builders<Domain.SellerAggregate.Seller>.Update.PullFilter(seller => seller.Products, Builders<Product>.Filter.Where(product => product.Id == context.Message.ProductId));
            var filter = Builders<Domain.SellerAggregate.Seller>.Filter.And(
                Builders<Domain.SellerAggregate.Seller>.Filter.Where(seller => seller.Id == context.Message.SellerId),
            Builders<Domain.SellerAggregate.Seller>.Filter.ElemMatch(i => i.Products, u => u.Id == context.Message.ProductId));

            var update = Builders<Domain.SellerAggregate.Seller>.Update
                .Set(l => l.Products[-1].Status, EntityStatus.Deleted)
                .Set(l => l.Products[-1].LastUpdatedTime, DateTime.Now)
                .Set(l => l.LastUpdatedTime, DateTime.Now);
            await _sellerRepository.UpdateOneAsync(filter, update);
            await _sellerRepository.UnitOfWork.SaveChangesAsync();
            _logger.LogInformation("Value: {Value}", context.Message);
            await _endpoint.Publish(new ProductDeletedEvent(context.Message.CorrelationId, context.Message.ProductId));
        }

      
    }
}
