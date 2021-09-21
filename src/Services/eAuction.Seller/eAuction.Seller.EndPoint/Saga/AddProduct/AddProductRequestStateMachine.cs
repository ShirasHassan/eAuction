using System;
using System.Threading.Tasks;
using Automatonymous;
using eAuction.AuctionBC.Contract.Commands;
using eAuction.Seller.Contract.Commands;
using eAuction.Seller.Contract.Query;
using eAuction.Seller.Message;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace eAuction.Seller.EndPoint.Saga.AddProduct
{

    public class AddProductRequestStateMachine :
       MassTransitStateMachine<AddProductRequestState>
    {
        public AddProductRequestStateMachine(ILogger<AddProductRequestStateMachine> logger, IEndpointNameFormatter formatter)
        {
            InstanceState(x => x.CurrentState);
            Event(() => AddProductRequest, e =>
            {
                e.CorrelateById(x => x.Message.CorrelationId);
                e.SelectId(c => NewId.NextGuid());
                e.InsertOnInitial = true;
                e.SetSagaFactory(context => InitializeState(context));
            });

            Event(() => SellerIdResponse, e => e.CorrelateById(x => x.Message.CorrelationId));
            Event(() => ProductAdded, e => e.CorrelateById(x => x.Message.CorrelationId));
            Event(() => SellerCreated, e => e.CorrelateById(x => x.Message.CorrelationId));

            Initially(
                When(AddProductRequest)
                .Then(x => logger.LogInformation($"{x.Instance.CorrelationId}: StateMachine has started processing"))
                .ThenAsync(async x => await SendGetSellerIdByEmailRequest(x))
                .TransitionTo(RequestReceived)
            );

            During(RequestReceived,
                When(SellerIdResponse)
                .Then(x => UpdateSellerId(x))
                .ThenAsync(async context => await StartAddingProduct(context))
                .TransitionTo(ProcessStarted));

            During(ProcessStarted,
                When(ProductAdded)
                .ThenAsync(async x => await StartAddingAuctionItem(x))
                .TransitionTo(Processing));

            During(ProcessStarted,
                When(SellerCreated)
                .ThenAsync(async x => await StartAddingAuctionItem(x))
                .TransitionTo(Processing));

            During(Processing,
                When(AuctionItemAdded)
                .ThenAsync(async x => await SendAddProductRequestResponse(x))
                .TransitionTo(ProcessCompleted));

            During(Processing,
                When(AuctionBCFailed)
                .ThenAsync(async x => await SendFailureResponse(x))
                .TransitionTo(ProcessFailed));

            DuringAny(
            When(ProcessCompleted.Enter)
              .Finalize());


        }

        private async Task SendFailureResponse(BehaviorContext<AddProductRequestState, CommandFailedEvent> context)
        {
            //Send response back to orignial requestor once we are done with this step
            if (context.Instance.ResponseAddress != null)
            {
                var responseEndpoint = await context.GetSendEndpoint(new Uri(context.Instance.ResponseAddress));
                var sellerId = context.Instance.SellerId;
                var productId = context.Instance.ProductId;
                await responseEndpoint.Send(new ProductAddedResponse()
                {
                    CorrelationId = context.Instance.CorrelationId,
                    SellerId = sellerId?.ToString(),
                    ProductId = productId?.ToString(),
                    Exception = new Message.Exception() { Message = context.Data.message },
                },
                    callback: sendContext => sendContext.RequestId = context.Instance.RequestId);
                context.Instance.LastUpdatedTime = DateTime.Now;
            }
        }

        private void UpdateSellerId(BehaviorContext<AddProductRequestState, GetSellerIdResponse> x)
        {
            x.Instance.SellerId = x.Data.SellerId;
        }

        public State RequestReceived { get; private set; }
        public State ProcessStarted { get; private set; }
        public State Processing { get; private set; }
        public State ProcessCompleted { get; private set; }
        public State ProcessFailed { get; private set; }
        public State ProductCreated { get; private set; }

        public Event<AddProductRequest> AddProductRequest { get; private set; }
        public Event<GetSellerIdResponse> SellerIdResponse { get; private set; }
        public Event<ProductAddedEvent> ProductAdded { get; private set;}
        public Event<SellerCreatedEvent> SellerCreated { get; private set; }
        public Event<AuctionItemAddedEvent> AuctionItemAdded { get; private set; }
        public Event<CommandFailedEvent> AuctionBCFailed { get; private set; }


        private async Task StartAddingProduct(BehaviorContext<AddProductRequestState, GetSellerIdResponse> context)
        {
            var sellerInfo = context.Instance.Request.Seller;
            var productInfo = context.Instance.Request.Product;
            var seller = new Domain.SellerAggregate.Seller(string.IsNullOrEmpty(context.Data.SellerId) ? Guid.NewGuid().ToString() : context.Data.SellerId,
                                                    sellerInfo.FirstName, sellerInfo.LastName, sellerInfo.Address,
                                                    sellerInfo.City, sellerInfo.State, sellerInfo.Pin, sellerInfo.Phone, sellerInfo.Email);

            seller.VerifyAndAddProduct(productInfo.ProductName, productInfo.ShortDescription,
                productInfo.DetailedDescription, productInfo.Category, double.Parse(productInfo.StartingPrice),
                productInfo.BidEndDate);
            if (context.Data.SellerId == string.Empty) {
               await context.Publish(new CreateSellerCommand(context.Data.CorrelationId,  seller ));
            }
            else {
                await context.Publish(new AddProductCommand(context.Data.CorrelationId, seller.Products[0] ,seller.Id));
            }
            context.Instance.SellerId = seller.Id;
            context.Instance.ProductId = seller.Products[0].Id;
            context.Instance.LastUpdatedTime = DateTime.Now;
        }

        private async Task SendGetSellerIdByEmailRequest(BehaviorContext<AddProductRequestState> context)
        {
            await context.Publish(new GetSellerIdByEmail(context.Instance.CorrelationId, context.Instance.Request.Seller.Email));
            context.Instance.LastUpdatedTime = DateTime.Now;
        }

        private async Task StartAddingAuctionItem<T>(BehaviorContext<AddProductRequestState, T> context)
        {
            await context.Publish(new AddAuctionItemCommand(context.Instance.CorrelationId, context.Instance.ProductId, context.Instance.SellerId,
                $"{context.Instance.Request.Seller.FirstName} {context.Instance.Request.Seller.LastName}", context.Instance.Request.Product.ProductName,
                context.Instance.Request.Product.ShortDescription, context.Instance.Request.Product.DetailedDescription, context.Instance.Request.Product.Category,
                Double.Parse(context.Instance.Request.Product.StartingPrice), context.Instance.Request.Product.BidEndDate));
            context.Instance.LastUpdatedTime = DateTime.Now;
        }

        private async Task SendAddProductRequestResponse<T>(BehaviorContext<AddProductRequestState, T> context)
        {
            //Send response back to orignial requestor once we are done with this step
            if (context.Instance.ResponseAddress != null)
            {
                var responseEndpoint = await context.GetSendEndpoint(new Uri(context.Instance.ResponseAddress));
                var sellerId = context.Instance.SellerId;
                var productId = context.Instance.ProductId;
                await responseEndpoint.Send(new ProductAddedResponse()
                {
                    CorrelationId = context.Instance.CorrelationId,
                    SellerId = sellerId.ToString(),
                    ProductId = productId.ToString()
                },
                    callback: sendContext => sendContext.RequestId = context.Instance.RequestId);
                context.Instance.LastUpdatedTime = DateTime.Now;
            }
        }

        private AddProductRequestState InitializeState(ConsumeContext<AddProductRequest> context)
        {
            return new AddProductRequestState()
            {
                ResponseAddress = context.ResponseAddress?.ToString(),
                RequestId = context.RequestId,
                CurrentState = Initial.Name,
                CorrelationId = context.Message.CorrelationId,
                LastUpdatedTime = DateTime.Now,
                RequestTime = DateTime.Now,
                ProductId = "",
                SellerId = "",
                Request = context.Message
            };
        }
    }
}
