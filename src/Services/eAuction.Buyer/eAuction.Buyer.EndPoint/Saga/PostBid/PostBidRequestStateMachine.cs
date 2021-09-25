using System;
using System.Threading.Tasks;
using Automatonymous;
using eAuction.AuctionBC.Contract.Commands;
using eAuction.Buyer.Contract.Commands;
using eAuction.Buyer.Contract.Message;
using eAuction.Buyer.Contract.Queries;
using MassTransit;
using Microsoft.Extensions.Logging;
using static eAuction.Buyer.Contract.Commands.CreateBuyerComand;

namespace eAuction.Buyer.EndPoint.Saga.PostBid
{
    public class PostBidRequestStateMachine :
       MassTransitStateMachine<PostBidRequestState>
    {
        public PostBidRequestStateMachine(ILogger<PostBidRequestStateMachine> logger, IEndpointNameFormatter formatter)
        {
            InstanceState(x => x.CurrentState);
            Event(() => AddAuctionRequest, e =>
            {
                e.CorrelateById(x => x.Message.CorrelationId);
                e.SelectId(c => NewId.NextGuid());
                e.InsertOnInitial = true;
                e.SetSagaFactory(context => InitializeState(context));
            });

            Event(() => BuyerIdResponse, e => e.CorrelateById(x => x.Message.CorrelationId));
            Event(() => BuyerBidPostedEvent, e => e.CorrelateById(x => x.Message.CorrelationId));
            Event(() => BuyerCreatedEvent, e => e.CorrelateById(x => x.Message.CorrelationId));
            Event(() => AuctionBidPostedEvent, e => e.CorrelateById(x => x.Message.CorrelationId));
            Event(() => AuctionBCFailed, e => e.CorrelateById(x => x.Message.CorrelationId));
 
            Initially(
                When(AddAuctionRequest)
                .Then(context => logger.LogInformation($"{context.Instance.CorrelationId}: StateMachine has started processing"))
                .ThenAsync(async context => await SendGetBuyerIdByEmailRequest(context))
                .TransitionTo(RequestReceived)
            );

            During(RequestReceived,
                When(BuyerIdResponse)
                .Then(context => UpdateBuyerId(context))
                .ThenAsync(async context => await PostBuyerBid(context))
                .TransitionTo(ProcessStarted));

            During(ProcessStarted,
                When(BuyerBidPostedEvent)
                .ThenAsync(async context => await PostAuctionBid(context))
                .TransitionTo(Processing));

            During(ProcessStarted,
                When(BuyerCreatedEvent)
                .ThenAsync(async context => await PostAuctionBid(context))
                .TransitionTo(Processing));

            During(Processing,
                When(AuctionBidPostedEvent)
                .ThenAsync(async context => await SendAuctionAddedResponse(context))
                .TransitionTo(ProcessCompleted));

            During(Processing,
                When(AuctionBCFailed)
                .ThenAsync(async context => await SendFailureResponse(context))
                .TransitionTo(ProcessFailed));

            DuringAny(
            When(ProcessCompleted.Enter)
              .Finalize());


        }

        private async Task SendFailureResponse(BehaviorContext<PostBidRequestState, CommandFailedEvent> context)
        {
            //Send response back to orignial requestor once we are done with this step
            if (context.Instance.ResponseAddress != null)
            {
                var responseEndpoint = await context.GetSendEndpoint(new Uri(context.Instance.ResponseAddress));
                var BuyerId = context.Instance.BuyerId;
                var productId = context.Instance.ProductId;
                await responseEndpoint.Send(new AuctionAddedResponse()
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

        
        public State RequestReceived { get; private set; }
        public State ProcessStarted { get; private set; }
        public State Processing { get; private set; }
        public State ProcessCompleted { get; private set; }
        public State ProcessFailed { get; private set; }
        public State BidPosted { get; private set; }

        public Event<AddAuctionRequest> AddAuctionRequest { get; private set; }
        public Event<GetBuyerIdResponse> BuyerIdResponse { get; private set; }
        public Event<Buyer.Contract.Commands.BidPostedEvent> BuyerBidPostedEvent { get; private set; }
        public Event<BuyerCreatedEvent> BuyerCreatedEvent { get; private set; }
        public Event<AuctionBC.Contract.Commands.BidPostedEvent> AuctionBidPostedEvent { get; private set; }
        public Event<CommandFailedEvent> AuctionBCFailed { get; private set; }


        private async Task PostBuyerBid(BehaviorContext<PostBidRequestState, GetBuyerIdResponse> context)
        {
            var Buyer = new Domain.BuyerAggregate.Buyer(string.IsNullOrEmpty(context.Data.BuyerId) ? Guid.NewGuid().ToString() : context.Data.BuyerId,
                                                    context.Instance.Request.FirstName, context.Instance.Request.LastName, context.Instance.Request.Address,
                                                    context.Instance.Request.City, context.Instance.Request.State, context.Instance.Request.Pin,
                                                    context.Instance.Request.Phone, context.Instance.Request.Email);

            Buyer.Bids.Add( new Domain.BuyerAggregate.AuctionItem(context.Instance.ProductId, double.Parse(context.Instance.Request.BidAmount)));
            if (context.Data.BuyerId == string.Empty)
            {
                await context.Publish(new CreateBuyerCommand(context.Data.CorrelationId, Buyer));
            }
            else
            {
                await context.Publish(new PostBidCommand(context.Data.CorrelationId, Buyer.Id, context.Instance.Request.ProductId, context.Instance.Request.BidAmount));
            }
            context.Instance.BuyerId = Buyer.Id;
            context.Instance.ProductId = context.Instance.Request.ProductId;
            context.Instance.LastUpdatedTime = DateTime.Now;
        }

        private void UpdateBuyerId(BehaviorContext<PostBidRequestState, GetBuyerIdResponse> x)
        {
            x.Instance.BuyerId = x.Data.BuyerId;
        }

        private async Task SendGetBuyerIdByEmailRequest(BehaviorContext<PostBidRequestState,AddAuctionRequest> context)
        {
            await context.Publish(new GetBuyerIdByEmail(context.Instance.CorrelationId, context.Instance.Request.Email));
            context.Instance.LastUpdatedTime = DateTime.Now;
        }

        private async Task PostAuctionBid<T>(BehaviorContext<PostBidRequestState, T> context)
        {
            await context.Publish(new BidCommand(context.Instance.CorrelationId, context.Instance.BuyerId,
                $"{context.Instance.Request.FirstName} {context.Instance.Request.LastName}", context.Instance.Request.Phone,
                context.Instance.Request.Email, context.Instance.ProductId,
                Double.Parse(context.Instance.Request.BidAmount)));
            context.Instance.LastUpdatedTime = DateTime.Now;
        }

        private async Task SendAuctionAddedResponse<T>(BehaviorContext<PostBidRequestState, T> context)
        {
            //Send response back to orignial requestor once we are done with this step
            if (context.Instance.ResponseAddress != null)
            {
                var responseEndpoint = await context.GetSendEndpoint(new Uri(context.Instance.ResponseAddress));
                var BuyerId = context.Instance.BuyerId;
                var productId = context.Instance.ProductId;
                await responseEndpoint.Send(new AuctionAddedResponse()
                {
                    CorrelationId = context.Instance.CorrelationId,
                    BuyerId = BuyerId.ToString(),
                    ProductId = productId.ToString()
                },
                    callback: sendContext => sendContext.RequestId = context.Instance.RequestId);
                context.Instance.LastUpdatedTime = DateTime.Now;
            }
        }

        private PostBidRequestState InitializeState(ConsumeContext<AddAuctionRequest> context)
        {
            return new PostBidRequestState()
            {
                ResponseAddress = context.ResponseAddress?.ToString(),
                RequestId = context.RequestId,
                CurrentState = Initial.Name,
                CorrelationId = context.Message.CorrelationId,
                LastUpdatedTime = DateTime.Now,
                RequestTime = DateTime.Now,
                ProductId = "",
                BuyerId = "",
                Request = context.Message
            };
        }
    }
}
