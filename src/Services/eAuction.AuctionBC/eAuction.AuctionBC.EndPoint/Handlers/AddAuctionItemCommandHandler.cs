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
    public class AddAuctionItemCommandHandler: IConsumer<AddAuctionItem.Command>
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
        public async Task Consume(ConsumeContext<AddAuctionItem.Command> context)
        {
            try
            {
                var auctionItem = new AuctionItem(context.Message.ProductId, context.Message.ItemName, context.Message.SellerId , context.Message.SellerName,
                context.Message.ShortDescription,
                context.Message.DetailedDescription,
                context.Message.Category, context.Message.StartingPrice, context.Message.BidEndDate);

                _auctionRepository.Add(auctionItem);
                await _auctionRepository.UnitOfWork.SaveChangesAsync();
                await _endpoint.Publish(new AddAuctionItem.SuccessEvent(context.Message.CorrelationId, context.Message.ProductId));
            }
            catch(Exception e)  {
                await _endpoint.Publish(new AddAuctionItem.FailedEvent(context.Message.CorrelationId, e.Message));
            }
        }
    }
}