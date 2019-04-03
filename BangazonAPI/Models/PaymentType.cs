using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class PaymentType
    {
        public int Id { get; set; }

        [Required]
        public int AcctNumber { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        public int CustomerId { get; set; }
    }
}

