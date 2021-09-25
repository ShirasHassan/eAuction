using System;
using AutoMapper;
using eAuction.AuctionBC.Contract.Queries;

namespace eAuction.AuctionBC.EndPoint
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Domain.AuctionItemAggregate.AuctionItem, AuctionItemModel>();
            CreateMap<Domain.AuctionItemAggregate.Bid, BidModel>();
        }
    }
}
