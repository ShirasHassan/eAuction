using System;
using System.Collections.Generic;
using System.Linq;
using eAuction.BaseLibrary.Domain;

namespace eAuction.Seller.Domain.SellerAggregate
{
    public class Seller : Entity, IAggregateRoot
    {
        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string Address { get; private set; }

        public string City { get; private set; }

        public string State { get; private set; }
        public string Pin { get; private set; }

        public string Phone { get; private set; }

        public string Email { get; private set; }

        public List<Product> Products { get; private set; }

        public Seller()
        {
            Products = new();
            Id = Guid.NewGuid().ToString();
        }

        public Seller(string identity, string fname,string lname,string address, string city, string state, string pin, string phone,string email) : this(fname,  lname,  address,  city,  state,  pin,  phone,  email)
        {
            Id = !string.IsNullOrWhiteSpace(identity) ? identity : throw new ArgumentNullException(nameof(identity));
           
        }

        public Seller(string fname, string lname, string address, string city, string state, string pin, string phone, string email):this()
        {
            FirstName = !string.IsNullOrWhiteSpace(fname) ? fname : throw new ArgumentNullException(nameof(fname));
            LastName = lname;
            Address = address;
            City = city;
            State = state;
            Pin = pin;
            Phone = !string.IsNullOrWhiteSpace(phone) ? phone : throw new ArgumentNullException(nameof(phone));
            Email = !string.IsNullOrWhiteSpace(email) ? email : throw new ArgumentNullException(nameof(email));
            
        }

        public Product VerifyAndAddProduct( string productName ,string shortDescription,string detailedDescription , string category ,double startingPrice, DateTime bidEndDate)
        {
            Product product = new(productName, shortDescription, detailedDescription, category, startingPrice, bidEndDate);
            Products.Add(product);
            return product;
        }

        public Product VerifyAndAddProduct(Product product)
        {
            if(Products.Any(p => p.Id == product.Id)) {
                throw new Exception("Item already added");
            }
            Products.Add(product);
            return product;
        }
        
        public void RemoveProduct(string productId)
        {
            var product = Products.FirstOrDefault(x => x.Id == Id);
            if (product != null) {
                Products.Remove(product);
            }
        }
    }
}
