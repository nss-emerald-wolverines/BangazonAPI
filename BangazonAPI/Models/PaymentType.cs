using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class PaymentType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AcctNumber { get; set; }

        [StringLength(15, ErrorMessage = "Maximum length is 15 characters"), Required]
        public string Name { get; set; }

        [Required]
        public int CustomerId { get; set; }
    }
}

