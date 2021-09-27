using System;
using System.Threading.Tasks;
using eAuction.Buyer.Contract.Queries;
using eAuction.Buyer.Domain.BuyerAggregate;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace eAuction.Buyer.EndPoint.Handlers
{
    public class GetBuyerIdByEmailQueryHandler : IConsumer<GetBuyerId.ByEmail>
    {

        readonly ILogger<GetBuyerIdByEmailQueryHandler> _logger;
        private readonly IBuyerRepository _buyerRepository;
        readonly IPublishEndpoint _endpoint;
        // readonly IRequestClient<GetBuyerIdByEmail> _requestClient;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="buyerRepository"></param>
        public GetBuyerIdByEmailQueryHandler(ILogger<GetBuyerIdByEmailQueryHandler> logger, IBuyerRepository buyerRepository,
            IPublishEndpoint endpoint)
        {
            _logger = logger;
            _buyerRepository = buyerRepository;
            // _requestClient = requestClient;
            _endpoint = endpoint;
        }

        /// <summary>
        /// Consumer/Handler
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<GetBuyerId.ByEmail> context)
        {

            var Buyer = await _buyerRepository.FindOneAsync(x => x.Email == context.Message.EmailId);
            _logger.LogInformation("Value: {Value}", context.Message);
            await context.RespondAsync(new GetBuyerId.Response(
                context.Message.CorrelationId,
                Buyer?.Id ?? string.Empty));
        }
    }
}
