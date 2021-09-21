using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eAuction.Seller.Message
{
    /// <summary>
    /// AddProductCommand
    /// </summary>
    public class AddProductRequest
    {
        public Guid CorrelationId { get; set; }
        /// <summary>
        /// Product
        /// </summary>
        [Required]
        public ProductModel Product { get; set; }
        
        /// <summary>
        /// Seller
        /// </summary>
        [Required]
        public SellerModel Seller { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ProductModel : IValidatableObject
    {
        
        private readonly List<string> _categories = new List<string> { "Painting", "Sculptor", "Ornament" };
        
        /// <summary>
        /// 
        /// </summary>
        [Required, StringLength(maximumLength: 30, ErrorMessage = "Product Name is not null", MinimumLength = 5)]
        public string ProductName { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string ShortDescription { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string DetailedDescription { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Category { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [RegularExpression("^[0-9]*$", ErrorMessage = "Starting price should be a number.")]
        public string StartingPrice { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public DateTime BidEndDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (!_categories.Contains(Category))
                results.Add(new ValidationResult("Invalid Category.", new[] { nameof(Category) }));

            if (BidEndDate > DateTime.Now)
                results.Add(new ValidationResult("Bid end date should be a future date.", new[] { nameof(BidEndDate) }));

            return results;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SellerModel
    {
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
    }

    /// <summary>
    /// 
    /// </summary>
    public class ProductAddedResponse
    {
        public Guid CorrelationId { get; set; }
        public string SellerId { get; set; }
        public string ProductId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Exception Exception { get; set; }
    }

    public class Exception
    {
       public string Message { get; set; }
    }
}
