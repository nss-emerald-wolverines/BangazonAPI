using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class OrderProduct
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int OrderId { get; set; }

        // public Product Product { get; set; } Not necessary
        // Just select and Join everything upfront
        // Don't need to add a product object to ProductOrder to access Products
    }
}