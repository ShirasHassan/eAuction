using System;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace eAuction.Buyer.EndPoint.Saga.PostBid
{
    public class PostBidRequestClassMap :
        BsonClassMap<PostBidRequestState>
    {
        public PostBidRequestClassMap()
        {
            MapProperty(x => x.RequestTime)
                .SetSerializer(new DateTimeSerializer(DateTimeKind.Utc));
            MapProperty(x => x.LastUpdatedTime)
                .SetSerializer(new DateTimeSerializer(DateTimeKind.Utc));

            MapProperty(x => x.CorrelationId);
            MapProperty(x => x.ProductId);
            MapProperty(x => x.BuyerId);
            MapProperty(x => x.Request);
            MapProperty(x => x.RequestId);
            MapProperty(x => x.ResponseAddress);
            MapProperty(x => x.CurrentState);
            MapProperty(x => x.Version);
        }
    }
}
