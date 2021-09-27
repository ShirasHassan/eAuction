using System;
using AutoMapper;
using eAuction.AuctionBC.Contract.Queries;

namespace eAuction.AuctionBC.EndPoint
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Domain.AuctionItemAggregate.AuctionItem, GetAuctionDetails.AuctionItemModel>();
            CreateMap<Domain.AuctionItemAggregate.Bid, GetAuctionDetails.BidModel>();
        }
    }
}
