using System.Threading.Tasks;
using eAuction.Seller.Contract.Commands;
using eAuction.Seller.Domain.SellerAggregate;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace eAuction.Seller.EndPoint.Handlers
{
    public class CreateSellerCommandHandler : IConsumer<CreateSellerCommand>
    {

        readonly ILogger<CreateSellerCommandHandler> _logger;
        private readonly ISellerRepository _sellerRepository;
        readonly IPublishEndpoint _endpoint;
        readonly IRequestClient<CreateSellerCommand> _requestClient;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="sellerRepository"></param>
        public CreateSellerCommandHandler(ILogger<CreateSellerCommandHandler> logger, ISellerRepository sellerRepository,
            IPublishEndpoint endpoint)
        {
            _logger = logger;
            _sellerRepository = sellerRepository;
          //  _requestClient = requestClient;
            _endpoint = endpoint;
        }

        /// <summary>
        /// Consumer/Handler
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<CreateSellerCommand> context)
        {
            _sellerRepository.Add(context.Message.Seller);
            await _sellerRepository.UnitOfWork.SaveChangesAsync();
            _logger.LogInformation("Value: {Value}", context.Message);
            await _endpoint.Publish(new SellerCreatedEvent(context.Message.CorrelationId, context.Message.Seller.Id));
        }
    }
}

