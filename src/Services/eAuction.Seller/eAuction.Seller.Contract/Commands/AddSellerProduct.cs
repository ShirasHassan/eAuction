using System;

namespace eAuction.Seller.Contract.Commands
{
    public interface AddSellerProduct
    {
        public Guid Id { get; }
        public Domain.SellerAggregate.Seller Seller { get; }
    }
}