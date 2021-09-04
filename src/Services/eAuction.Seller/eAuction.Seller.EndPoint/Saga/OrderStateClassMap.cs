using System;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace eAuction.Seller.EndPoint.Saga
{
    class OrderStateClassMap :
        BsonClassMap<OrderState>
    {
        public OrderStateClassMap()
        {
            MapProperty(x => x.OrderDate)
                .SetSerializer(new DateTimeSerializer(DateTimeKind.Utc));
        }
    }
}
