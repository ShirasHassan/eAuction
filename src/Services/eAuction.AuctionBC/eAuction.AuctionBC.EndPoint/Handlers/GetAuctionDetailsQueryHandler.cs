using System;
using System.Threading.Tasks;
using AutoMapper;
using eAuction.AuctionBC.Contract.Queries;
using eAuction.AuctionBC.Domain.AuctionItemAggregate;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace eAuction.AuctionBC.EndPoint.Handlers
{
    public class GetAuctionDetailsQueryHandler: IConsumer<GetAuctionDetailsQuery>
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
        public async Task Consume(ConsumeContext<GetAuctionDetailsQuery> context)
        {
            try
            {
                var auctionItem = await _auctionRepository.FindOneAsync(x => x.Id == context.Message.AuctionItemId);
                _logger.LogInformation("Value: {Value}", context.Message);
                if (auctionItem != null) {
                    await context.RespondAsync(new AuctionDetails(context.Message.CorrelationId, _mapper.Map<AuctionItemModel>(auctionItem)));
                }
                
            }
            catch (Exception e)
            {
                await context.RespondAsync(new AuctionDetails(context.Message.CorrelationId, null));
            }


        }
    }
}