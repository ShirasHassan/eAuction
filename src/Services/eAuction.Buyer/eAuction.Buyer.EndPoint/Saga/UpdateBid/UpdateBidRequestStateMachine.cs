using System;
using System.Threading.Tasks;
using Automatonymous;
using eAuction.AuctionBC.Contract.Commands;
using eAuction.Buyer.Contract.Commands;
using eAuction.Buyer.Contract.Message;
using eAuction.Buyer.Contract.Queries;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace eAuction.Buyer.EndPoint.Saga.UpdateBid
{
    public class UpdateBidRequestStateMachine :
       MassTransitStateMachine<UpdateBidRequestState>
    {
        public UpdateBidRequestStateMachine(ILogger<UpdateBidRequestStateMachine> logger, IEndpointNameFormatter formatter)
        {
            InstanceState(x => x.CurrentState);
            Event(() => UpdateAuctionRequest, e =>
            {
                e.CorrelateById(x => x.Message.CorrelationId);
                e.SelectId(c => NewId.NextGuid());
                e.InsertOnInitial = true;
                e.SetSagaFactory(context => InitializeState(context));
            });

            Event(() => BuyerIdResponse, e => e.CorrelateById(x => x.Message.CorrelationId));
            Event(() => UpdateAuctionRequest, e => e.CorrelateById(x => x.Message.CorrelationId));
            Event(() => BuyerAmountUpdatedEvent, e => e.CorrelateById(x => x.Message.CorrelationId));
            Event(() => AuctionBidAmountUpdatedEvent, e => e.CorrelateById(x => x.Message.CorrelationId));
            Event(() => AuctionBCFailed, e => e.CorrelateById(x => x.Message.CorrelationId));


        Initially(
                When(UpdateAuctionRequest)
                .Then(async context => await SendGetBuyerIdByEmailRequest(context))
                .TransitionTo(RequestReceived));

            During(RequestReceived,
                When(BuyerIdResponse)
                .Then(context => UpdateBuyerId(context))
                .ThenAsync(async context => await UpdateBuyerBid(context))
                .TransitionTo(ProcessStarted));

            During(ProcessStarted,
                When(BuyerAmountUpdatedEvent)
                .ThenAsync(async context => await UpdateAuctionBidAmount(context))
                .TransitionTo(Processing));

            During(Processing,
                When(AuctionBidAmountUpdatedEvent)
                .ThenAsync(async context => await SendAuctionUpdatedResponse(context))
                .TransitionTo(ProcessCompleted));


            During(Processing,
                When(AuctionBCFailed)
                .ThenAsync(async context => await SendFailureResponse(context))
                .TransitionTo(ProcessFailed));

            DuringAny(
            When(ProcessCompleted.Enter)
              .Finalize());


        }

        public State RequestReceived { get; private set; }
        public State ProcessStarted { get; private set; }
        public State Processing { get; private set; }
        public State ProcessCompleted { get; private set; }
        public State ProcessFailed { get; private set; }
        public State BidUpdated { get; private set; }


        public Event<UpdateAuctionRequest> UpdateAuctionRequest { get; private set; }
        public Event<GetBuyerIdResponse> BuyerIdResponse { get; private set; }
        public Event<Buyer.Contract.Commands.UpdatedBidEvent> BuyerAmountUpdatedEvent { get; private set; }
        public Event<AuctionBC.Contract.Commands.BidAmountUpdatedEvent> AuctionBidAmountUpdatedEvent { get; private set; }
        public Event<BidAmountUpdateFailedEvent> AuctionBCFailed { get; private set; }

        private void UpdateBuyerId(BehaviorContext<UpdateBidRequestState, GetBuyerIdResponse> x)
        {
            x.Instance.BuyerId = x.Data.BuyerId;
        }

        private async Task SendGetBuyerIdByEmailRequest(BehaviorContext<UpdateBidRequestState, UpdateAuctionRequest> context)
        {
            await context.Publish(new GetBuyerIdByEmail(context.Instance.CorrelationId, context.Instance.Request.BuyerEmailId));
            context.Instance.LastUpdatedTime = DateTime.Now;
        }

        private async Task SendFailureResponse(BehaviorContext<UpdateBidRequestState, BidAmountUpdateFailedEvent> context)
        {
            //Send response back to orignial requestor once we are done with this step
            if (context.Instance.ResponseAddress != null)
            {
                var responseEndpoint = await context.GetSendEndpoint(new Uri(context.Instance.ResponseAddress));
                var BuyerId = context.Instance.BuyerId;
                var productId = context.Instance.ProductId;
                await responseEndpoint.Send(new AuctionUpdatedResponse()
                {
                    CorrelationId = context.Instance.CorrelationId,
                    BuyerId = BuyerId?.ToString(),
                    ProductId = productId?.ToString(),
                    Exception = new Contract.Message.Exception() { Message = context.Data.Message },
                },
                    callback: sendContext => sendContext.RequestId = context.Instance.RequestId);
                context.Instance.LastUpdatedTime = DateTime.Now;
            }
        }

        private async Task SendAuctionUpdatedResponse(BehaviorContext<UpdateBidRequestState, BidAmountUpdatedEvent> context)
        {
            if (context.Instance.ResponseAddress != null)
            {
                var responseEndpoint = await context.GetSendEndpoint(new Uri(context.Instance.ResponseAddress));
                var BuyerId = context.Instance.BuyerId;
                var productId = context.Instance.ProductId;
                await responseEndpoint.Send(new AuctionUpdatedResponse()
                {
                    CorrelationId = context.Instance.CorrelationId,
                    BuyerId = BuyerId.ToString(),
                    ProductId = productId.ToString()
                },
                    callback: sendContext => sendContext.RequestId = context.Instance.RequestId);
                context.Instance.LastUpdatedTime = DateTime.Now;
            }
        }

        private async Task UpdateAuctionBidAmount(BehaviorContext<UpdateBidRequestState, UpdatedBidEvent> context)
        {
            await context.Publish(new BidAmountUpdateCommand(context.Instance.CorrelationId, context.Instance.BuyerId, context.Instance.ProductId,
                double.Parse(context.Instance.Request.BidAmount)));
            context.Instance.LastUpdatedTime = DateTime.Now;
        }

        private async Task UpdateBuyerBid<T>(BehaviorContext<UpdateBidRequestState, T> context)
        {
            await context.Publish(new UpdateBidRequestCommand(context.Instance.CorrelationId, context.Instance.BuyerId, context.Instance.ProductId,
                double.Parse(context.Instance.Request.BidAmount)));
            context.Instance.LastUpdatedTime = DateTime.Now;
        }


        private UpdateBidRequestState InitializeState(ConsumeContext<UpdateAuctionRequest> context)
        {
            return new UpdateBidRequestState()
            {
                ResponseAddress = context.ResponseAddress?.ToString(),
                RequestId = context.RequestId,
                CurrentState = Initial.Name,
                CorrelationId = context.Message.CorrelationId,
                LastUpdatedTime = DateTime.Now,
                RequestTime = DateTime.Now,
                ProductId = context.Message.ProductId,
                BuyerId = "",
                Request = context.Message
            };
        }
    }
}