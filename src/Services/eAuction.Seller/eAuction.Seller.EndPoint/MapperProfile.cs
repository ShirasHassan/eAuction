using System;
using AutoMapper;
using eAuction.Seller.Message;

namespace eAuction.Seller.EndPoint
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap< Seller.Domain.SellerAggregate.Product, ProductInfo>();
        }
}
}