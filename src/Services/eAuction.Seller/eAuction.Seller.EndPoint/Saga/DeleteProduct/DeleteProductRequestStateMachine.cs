using System;
using System.Threading.Tasks;
using Automatonymous;
using eAuction.AuctionBC.Contract.Commands;
using eAuction.Seller.Contract.Commands;
using eAuction.Seller.Message;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace eAuction.Seller.EndPoint.Saga.DeleteProduct
{
    public class DeleteProductRequestStateMachine :
       MassTransitStateMachine<DeleteProductRequestState>
    {
        public DeleteProductRequestStateMachine(ILogger<DeleteProductRequestStateMachine> logger)
        {
            InstanceState(x => x.CurrentState);
            Event(() => DeleteProductRequest, e =>
            {
                e.CorrelateById(x => x.Message.CorrelationId);
                e.SelectId(c => NewId.NextGuid());
                e.InsertOnInitial = true;
                e.SetSagaFactory(context => InitializeState(context));
            });

            Event(() => ProductDeletedEvent, e => e.CorrelateById(x => x.Message.CorrelationId));
            Event(() => AuctionItemDeletedEvent, e => e.CorrelateById(x => x.Message.CorrelationId));
            Event(() => AuctionBCFailedEvent, e => e.CorrelateById(x => x.Message.CorrelationId));

            Initially(
                When(DeleteProductRequest)
                .Then(x => logger.LogInformation($"{x.Instance.CorrelationId}: StateMachine has started processing"))
                .ThenAsync(async x => await SendAuctionItemDeleteRequest(x))
                .TransitionTo(RequestReceived));

            During(RequestReceived,
                When(AuctionItemDeletedEvent)
                .Then(x => UpdateSellerId(x))
                .ThenAsync(async context => await SendProductDeleteCommand(context))
                .TransitionTo(ProcessStarted));

            During(RequestReceived,
                When(AuctionBCFailedEvent)
                .ThenAsync(async context => await SendFailureResponse(context))
                .TransitionTo(ProcessCompleted));

            During(ProcessStarted,
                When(ProductDeletedEvent)
                .ThenAsync(async context => await SendDeleteProductRequestResponse(context))
                .TransitionTo(ProcessCompleted));

            DuringAny(
            When(ProcessCompleted.Enter)
              .Finalize());
        }

        private  async Task SendDeleteProductRequestResponse(BehaviorContext<DeleteProductRequestState, ProductDeletedEvent> context)
        {
            if (context.Instance.ResponseAddress != null)
            {
                var responseEndpoint = await context.GetSendEndpoint(new Uri(context.Instance.ResponseAddress));
                await responseEndpoint.Send(new ProductDeletedResponse()
                {
                    CorrelationId = context.Instance.CorrelationId,
                    ProductId = context.Instance.ProductId,
                    SellerId = context.Instance.SellerId
                },
                    callback: sendContext => sendContext.RequestId = context.Instance.RequestId);
                context.Instance.LastUpdatedTime = DateTime.Now;
            }
        }

        private async Task SendFailureResponse(BehaviorContext<DeleteProductRequestState, CommandFailedEvent> context)
        {
            if (context.Instance.ResponseAddress != null)
            {
                var responseEndpoint = await context.GetSendEndpoint(new Uri(context.Instance.ResponseAddress));
                await responseEndpoint.Send(new ProductDeletedResponse()
                {
                    CorrelationId = context.Instance.CorrelationId,
                    Exception =  new Message.Exception() { Message = context.Data.message }
                },
                    callback: sendContext => sendContext.RequestId = context.Instance.RequestId);
                context.Instance.LastUpdatedTime = DateTime.Now;
            }
        }

        private void UpdateSellerId(BehaviorContext<DeleteProductRequestState, AuctionItemDeletedEvent> context)
        {
            context.Instance.SellerId = context.Data.SellerId;
            context.Instance.LastUpdatedTime = DateTime.Now;
        }

        private async Task SendProductDeleteCommand(BehaviorContext<DeleteProductRequestState, AuctionItemDeletedEvent> context)
        {
            await context.Publish(new DeleteProductCommand(context.Instance.CorrelationId, context.Instance.ProductId, context.Data.SellerId));
            context.Instance.LastUpdatedTime = DateTime.Now;
        }

        private async Task SendAuctionItemDeleteRequest(BehaviorContext<DeleteProductRequestState, ProductDeletedRequest> context)
        {
            await context.Publish(new DeleteAuctionItemCommand(context.Instance.CorrelationId, context.Instance.ProductId));
            context.Instance.LastUpdatedTime = DateTime.Now;
        }

        private DeleteProductRequestState InitializeState(ConsumeContext<ProductDeletedRequest> context)
        {
            return new DeleteProductRequestState()
            {
                ResponseAddress = context.ResponseAddress?.ToString(),
                RequestId = context.RequestId,
                CurrentState = Initial.Name,
                CorrelationId = context.Message.CorrelationId,
                LastUpdatedTime = DateTime.Now,
                RequestTime = DateTime.Now,
                ProductId = context.Message.ProductId,
                SellerId = ""
            };
        }

        public State RequestReceived { get; private set; }
        public State ProcessStarted { get; private set; }
        public State Processing { get; private set; }
        public State ProcessCompleted { get; private set; }
        public State ProcessFailed { get; private set; }
        public State ProductDeleted { get; private set; }

        public Event<ProductDeletedRequest> DeleteProductRequest { get; private set; }
        public Event<ProductDeletedEvent> ProductDeletedEvent { get; private set; }
        public Event<AuctionItemDeletedEvent> AuctionItemDeletedEvent { get; private set; }
        public Event<CommandFailedEvent> AuctionBCFailedEvent { get; private set; }
    }
}
