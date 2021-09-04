using System;
using System.Xml;
using Automatonymous;

namespace eAuction.Seller.EndPoint.Saga
{
    public class OrderStateMachine :
       MassTransitStateMachine<OrderState>
    {
        public OrderStateMachine()
        {
            InstanceState(x => x.CurrentState);
        }

        public int Processing { get; set; }
        public int Completed { get; set; }
    }
}
