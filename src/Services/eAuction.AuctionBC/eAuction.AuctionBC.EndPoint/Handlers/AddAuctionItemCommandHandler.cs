using System;
using System.Threading.Tasks;
using eAuction.AuctionBC.Contract.Commands;
using eAuction.AuctionBC.Domain.AuctionItemAggregate;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace eAuction.AuctionBC.EndPoint.Handlers
{
    /// <summary>
    /// AddAuctionItemCommandHandler
    /// </summary>
    public class AddAuctionItemCommandHandler: IConsumer<AddAuctionItemCommand>
    {

        readonly ILogger<AddAuctionItemCommandHandler> _logger;
        private readonly IAuctionRepository _auctionRepository;
        readonly IPublishEndpoint _endpoint;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="auctionRepository"></param>
        public AddAuctionItemCommandHandler(ILogger<AddAuctionItemCommandHandler> logger, IAuctionRepository auctionRepository, IPublishEndpoint endpoint)
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
        public async Task Consume(ConsumeContext<AddAuctionItemCommand> context)
        {
            try
            {
                var auctionItem = new AuctionItem(context.Message.ItemId, context.Message.ProductName, context.Message.SellerId , context.Message.SellerName,
                context.Message.ShortDescription,
                context.Message.DetailedDescription,
                context.Message.Category, context.Message.StartingPrice, context.Message.BidEndDate);

                _auctionRepository.Add(auctionItem);
                await _auctionRepository.UnitOfWork.SaveChangesAsync();
                await _endpoint.Publish(new AuctionItemAddedEvent(context.Message.CorrelationId, context.Message.ItemId));
            }
            catch(Exception e)
            {
                await _endpoint.Publish(new CommandFailedEvent(context.Message.CorrelationId, e.Message));
            }
        }
    }
}