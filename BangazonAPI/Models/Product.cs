﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public int ProductTypeId { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Title { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Description { get; set; }
        [Required]
        public int Quantity { get; set; }
    }


}
