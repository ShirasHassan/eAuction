using System;
using System.Threading.Tasks;
using eAuction.Buyer.Domain.BuyerAggregate;
using MassTransit;
using Microsoft.Extensions.Logging;
using static eAuction.Buyer.Contract.Commands.CreateBuyerComand;

namespace eAuction.Buyer.EndPoint.Handlers
{
    public class CreateBuyerCommandHandler : IConsumer<CreateBuyerCommand>
    {

        readonly ILogger<CreateBuyerCommandHandler> _logger;
        private readonly IBuyerRepository _BuyerRepository;
        readonly IPublishEndpoint _endpoint;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="BuyerRepository"></param>
        public CreateBuyerCommandHandler(ILogger<CreateBuyerCommandHandler> logger, IBuyerRepository BuyerRepository,
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
        public async Task Consume(ConsumeContext<CreateBuyerCommand> context)
        {
            _BuyerRepository.Add(context.Message.Buyer);
            await _BuyerRepository.UnitOfWork.SaveChangesAsync();
            _logger.LogInformation("Value: {Value}", context.Message);
            await _endpoint.Publish(new BuyerCreatedEvent(context.Message.CorrelationId, context.Message.Buyer.Id));
        }
    }
}

