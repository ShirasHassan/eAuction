using System.Threading.Tasks;
using eAuction.Seller.Contract.Query;
using eAuction.Seller.Domain.SellerAggregate;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace eAuction.Seller.EndPoint.Handlers
{
    public class GetSellerIdByEmailQueryHandler : IConsumer<GetSellerId.ByEmail>
    {

        readonly ILogger<GetSellerIdByEmailQueryHandler> _logger;
        private readonly ISellerRepository _sellerRepository;
        readonly IPublishEndpoint _endpoint;
       // readonly IRequestClient<GetSellerIdByEmail> _requestClient;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="sellerRepository"></param>
        public GetSellerIdByEmailQueryHandler(ILogger<GetSellerIdByEmailQueryHandler> logger, ISellerRepository sellerRepository,
            IPublishEndpoint endpoint)
        {
            _logger = logger;
            _sellerRepository = sellerRepository;
           // _requestClient = requestClient;
            _endpoint = endpoint;
        }

        /// <summary>
        /// Consumer/Handler
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<GetSellerId.ByEmail> context)
        {

            var seller = await _sellerRepository.FindOneAsync(x => x.Email == context.Message.EmailId);
            _logger.LogInformation("Value: {Value}", context.Message);
            await context.RespondAsync(new GetSellerId.Response(
                context.Message.CorrelationId,
                seller?.Id ?? string.Empty));
        }
    }
}
