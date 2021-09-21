using System;
using Automatonymous;
using eAuction.Buyer.Contract.Message;
using MassTransit.Saga;

namespace eAuction.Buyer.EndPoint.Saga.UpdateBid
{
    public class UpdateBidRequestState: SagaStateMachineInstance, ISagaVersion
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
        public DateTime RequestTime { get; set; }
        public DateTime LastUpdatedTime { get; set; }
        public int Version { get; set; }
        public UpdateAuctionRequest Request { get; set; }
        public string BuyerId { get; set; }
        public string ProductId { get; set; }
        public string ResponseAddress { get; internal set; }
        public Guid? RequestId { get; internal set; }

    }
}