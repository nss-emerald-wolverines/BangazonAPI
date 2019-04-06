using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class Order
    {
        private Customer _customer;

        [Key]
        public int OrderId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public int PaymentTypeId { get; set; }
        // public int? CustomerPaymentId { get; set; } // ? means that the variable can be null????

        public Customer Customer { get; set; }        

        // public Customer Customer { get => _customer; set => _customer = value; }

        public PaymentType PaymentType { get; set; } // Not needed for this sprint

        public List<Product> ListofProducts { get; set; } = new List<Product>();
    }
}
