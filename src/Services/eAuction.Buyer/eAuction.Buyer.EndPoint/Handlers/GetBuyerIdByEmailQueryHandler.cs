using System;
using System.Threading.Tasks;
using eAuction.Buyer.Contract.Queries;
using eAuction.Buyer.Domain.BuyerAggregate;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace eAuction.Buyer.EndPoint.Handlers
{
    public class GetBuyerIdByEmailQueryHandler : IConsumer<GetBuyerIdByEmail>
    {

        readonly ILogger<GetBuyerIdByEmailQueryHandler> _logger;
        private readonly IBuyerRepository _BuyerRepository;
        readonly IPublishEndpoint _endpoint;
        // readonly IRequestClient<GetBuyerIdByEmail> _requestClient;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="BuyerRepository"></param>
        public GetBuyerIdByEmailQueryHandler(ILogger<GetBuyerIdByEmailQueryHandler> logger, IBuyerRepository BuyerRepository,
            IPublishEndpoint endpoint)
        {
            _logger = logger;
            _BuyerRepository = BuyerRepository;
            // _requestClient = requestClient;
            _endpoint = endpoint;
        }

        /// <summary>
        /// Consumer/Handler
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<GetBuyerIdByEmail> context)
        {

            var Buyer = await _BuyerRepository.FindOneAsync(x => x.Email == context.Message.EmailId);
            _logger.LogInformation("Value: {Value}", context.Message);
            await context.RespondAsync(new GetBuyerIdResponse(
                context.Message.CorrelationId,
                Buyer?.Id ?? string.Empty));
        }
    }
}
