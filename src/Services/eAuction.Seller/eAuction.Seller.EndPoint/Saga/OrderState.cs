using System;
using Automatonymous;
using MassTransit.Saga;

namespace eAuction.Seller.EndPoint.Saga
{
    public class OrderState :
    SagaStateMachineInstance, ISagaVersion
    {
        public Guid CorrelationId { get; set; }
        public int CurrentState { get; set; }
        public DateTime? OrderDate { get; set; }
        public int Version { get; set; }
    }

   
}
