using eAuction.Seller.Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eAuction.Seller.Domain.SellerAggregate
{
    public class Seller : Entity, IAggregateRoot
    {
        public string IdentityGuid { get; private set; }
        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string Address { get; private set; }

        public string City { get; private set; }

        public string State { get; private set; }
        public string Pin { get; private set; }

        public string Phone { get; private set; }

        public string Email { get; private set; }

        public List<Product> Products { get; private set; }

        public Seller( string identity, string fname,string lname,string address, string city, string state, string pin, string phone,string email) : this()
        {
            IdentityGuid = !string.IsNullOrWhiteSpace(identity) ? identity : throw new ArgumentNullException(nameof(identity));
            FirstName = !string.IsNullOrWhiteSpace(fname) ? fname : throw new ArgumentNullException(nameof(fname));
            LastName = lname;
            Address = address;
            City = city;
            State = state;
            Pin = pin;
            Phone = !string.IsNullOrWhiteSpace(phone) ? phone : throw new ArgumentNullException(nameof(phone));
            Email = !string.IsNullOrWhiteSpace(email) ? email : throw new ArgumentNullException(nameof(email));
        }

        public Seller()
        {
            Products = new();
        }

        public Product VerifyAndAddProduct( string productName ,string shortDescription,string detailedDescription , string category ,double startingPrice, DateTime bidEndDate)
        {
            Product product = new(productName, shortDescription, detailedDescription, category, startingPrice, bidEndDate);
            Products.Add(product);
            return product;
        }

        public void RemoveProduct(string productId)
        {

        }
    }
}
