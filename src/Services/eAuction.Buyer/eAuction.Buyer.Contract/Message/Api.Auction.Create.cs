using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace eAuction.Buyer.Contract.Message
{
	 /// <summary>
    /// AddProductCommand
    /// </summary>
    public class AddAuctionRequest
    {
        public Guid CorrelationId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required, StringLength(maximumLength: 30, ErrorMessage = "First Name is not null", MinimumLength = 5)]
        public string FirstName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required, StringLength(maximumLength: 25, ErrorMessage = "Last Name is not null", MinimumLength = 5)]
        public string LastName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Pin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Phone]
        public string Phone { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [EmailAddress]
        public string Email { get; set; }

        public string ProductId { get; set; }

        public string BidAmount { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AuctionAddedResponse
    {
        public Guid CorrelationId { get; set; }
        public string BuyerId { get; set; }
        public string ProductId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Exception Exception { get; set; }
    }

    public class Exception
    {
       public string Message { get; set; }
    }
}
