using System;
using Automatonymous;
using MassTransit.Saga;

namespace eAuction.Seller.EndPoint.Saga.DeleteProduct
{
    public class DeleteProductRequestState : SagaStateMachineInstance, ISagaVersion
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
        public DateTime RequestTime { get; set; }
        public DateTime LastUpdatedTime { get; set; }
        public int Version { get; set; }
        public string SellerId { get; set; }
        public string ProductId { get; set; }
        public string ResponseAddress { get; internal set; }
        public Guid? RequestId { get; internal set; }
    }
    
}
