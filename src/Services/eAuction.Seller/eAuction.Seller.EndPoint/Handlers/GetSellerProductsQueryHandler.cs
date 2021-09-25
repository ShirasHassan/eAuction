using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using eAuction.Seller.Domain.SellerAggregate;
using eAuction.Seller.Message;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace eAuction.Seller.EndPoint.Handlers
{
    public class GetSellerProductsQueryHandler : IConsumer<ListProductRequest>
    {

        readonly ILogger<GetSellerProductsQueryHandler> _logger;
        private readonly ISellerRepository _sellerRepository;
        readonly IPublishEndpoint _endpoint;
        // readonly IRequestClient<GetProductRequest> _requestClient;
        private readonly IMapper _mapper;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="sellerRepository"></param>
        public GetSellerProductsQueryHandler(ILogger<GetSellerProductsQueryHandler> logger, ISellerRepository sellerRepository,
            IPublishEndpoint endpoint,IMapper mapper)
        {
            _mapper = mapper;
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
        public async Task Consume(ConsumeContext<ListProductRequest> context)
        {

            var seller = await _sellerRepository.FindOneAsync(x => x.Email == context.Message.EmailId);
            _logger.LogInformation("Value: {Value}", context.Message);
            await context.RespondAsync(new ListProductResponse() { CorrelationId = context.Message.CorrelationId, Products = _mapper.Map<List<ProductInfo>>(seller?.Products)});
        }
    }
}
