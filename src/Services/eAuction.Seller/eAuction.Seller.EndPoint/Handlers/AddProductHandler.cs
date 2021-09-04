using System;
using System.Threading.Tasks;
using eAuction.Seller.Contract.Commands;
using eAuction.Seller.Contract.Messages;
using eAuction.Seller.Domain.SellerAggregate;
using MassTransit;
using Microsoft.Extensions.Logging;
using Response = eAuction.Seller.Contract.Messages.Response;

namespace eAuction.Seller.EndPoint.Handlers
{
    /// <summary>
    /// AddProductHandler
    /// </summary>
    public class AddProductHandler : IConsumer<AddSellerProduct>
    {

        readonly ILogger<AddProductHandler> _logger;
        private readonly ISellerRepository _sellerRepository;
        
        /// <summary>
        /// EventConsumer
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="sellerRepository"></param>
        public AddProductHandler(ILogger<AddProductHandler> logger,ISellerRepository sellerRepository)
        {
            _logger = logger;
            _sellerRepository = sellerRepository;
        }

        /// <summary>
        /// Consumer/Handler
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<AddSellerProduct> context)
        {
           var seller = await _sellerRepository.GetById(context.Message.Seller.Id);
           if (seller != null) {
               context.Message.Seller.Products.ForEach(p => seller.VerifyAndAddProduct(p));
                _sellerRepository.Update(seller);
            }
            else
            {
                _sellerRepository.Add(context.Message.Seller);
            }
          
          await _sellerRepository.UnitOfWork.SaveChangesAsync();
          await context.RespondAsync<Response>(new {Message = $"Item {context.Message.Id} is proceeded",State = "Proceeded" });
          _logger.LogInformation("Value: {Value}", context.Message.Id);
        }
    }
}
