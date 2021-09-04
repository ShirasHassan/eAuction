using System;
namespace eAuction.Seller.Contract.Messages
{
    /// <summary>
    /// Response
    /// </summary>
    public interface Response
    {
        /// <summary>
        /// 
        /// </summary>
        string Message { get; }
        
        string State { get; }
    }


   
}
