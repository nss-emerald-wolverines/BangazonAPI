using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int PaymentTypeId { get; set; }

        public Customer Customer { get; set; }

        public PaymentType PaymentType { get; set; }

        // public List<Exercise> Exercises { get; set; } = new List<Exercise>();
    }
}
