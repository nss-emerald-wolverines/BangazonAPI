using System;
using System.Collections.Generic;
<<<<<<< HEAD
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

// Making the basic class with no constructor at the moment.
=======
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

>>>>>>> master
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

<<<<<<< HEAD
        public int CustomerId { get; set; }
    }
}
=======
        [Required]
        public int CustomerId { get; set; }
    }
}

>>>>>>> master
