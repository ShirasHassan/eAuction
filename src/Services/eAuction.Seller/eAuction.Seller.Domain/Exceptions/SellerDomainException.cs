using System;

namespace eAuction.Seller.Domain.Exceptions
{
    /// <summary>
    /// Exception type for domain exceptions
    /// </summary>
    public class SellerDomainException : Exception
    {
        public SellerDomainException()
        { }

        public SellerDomainException(string message)
            : base(message)
        { }

        public SellerDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
