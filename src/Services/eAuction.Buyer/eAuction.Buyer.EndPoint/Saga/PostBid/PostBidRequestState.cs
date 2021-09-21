using System;
using Automatonymous;
using eAuction.Buyer.Contract.Message;
using MassTransit.Saga;

namespace eAuction.Buyer.EndPoint.Saga.PostBid
{
    public class PostBidRequestState : SagaStateMachineInstance, ISagaVersion
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
        public DateTime RequestTime { get; set; }
        public DateTime LastUpdatedTime { get; set; }
        public int Version { get; set; }
        public AddAuctionRequest Request { get; set; }
        public string BuyerId { get; set; }
        public string ProductId { get; set; }
        public string ResponseAddress { get; internal set; }
        public Guid? RequestId { get; internal set; }

    }
}

