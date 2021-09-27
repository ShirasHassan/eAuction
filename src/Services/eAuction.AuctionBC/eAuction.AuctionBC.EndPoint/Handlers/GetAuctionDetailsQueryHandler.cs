using System;
using System.Threading.Tasks;
using AutoMapper;
using eAuction.AuctionBC.Contract.Queries;
using eAuction.AuctionBC.Domain.AuctionItemAggregate;
using eAuction.BaseLibrary.Domain;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace eAuction.AuctionBC.EndPoint.Handlers
{
    public class GetAuctionDetailsQueryHandler: IConsumer<GetAuctionDetails.ByProductId>
    {

        readonly ILogger<GetAuctionDetailsQueryHandler> _logger;
        private readonly IAuctionRepository _auctionRepository;
        readonly IPublishEndpoint _endpoint;
        private readonly IMapper _mapper;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="auctionRepository"></param>
        public GetAuctionDetailsQueryHandler(ILogger<GetAuctionDetailsQueryHandler> logger, IAuctionRepository auctionRepository, IPublishEndpoint endpoint, IMapper mapper)
        {
            _mapper = mapper;
            _logger = logger;
            _auctionRepository = auctionRepository;
            _endpoint = endpoint;
        }

        /// <summary>
        /// Consumer/Handler
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<GetAuctionDetails.ByProductId> context)
        {
            try
            {
                var auctionItem = await _auctionRepository.FindOneAsync(x => x.Id == context.Message.ProductId && x.Status.Id == EntityStatus.Active.Id);
                _logger.LogInformation("Value: {Value}", context.Message);
                await context.RespondAsync(new GetAuctionDetails.Response(context.Message.CorrelationId, _mapper.Map<GetAuctionDetails.AuctionItemModel>(auctionItem)));
            }
            catch (Exception e) {
                await context.RespondAsync(new GetAuctionDetails.Response(context.Message.CorrelationId, null));
            }


        }
    }
}