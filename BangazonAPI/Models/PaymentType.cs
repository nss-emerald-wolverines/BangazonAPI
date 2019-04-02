using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Making the basic class with no constructor at the moment.
namespace BangazonAPI.Models
{
    public class PaymentType
    {
        public int Id { get; set; }

        public int AcctNumber { get; set; }

        public string Name { get; set; }

        public int CustomerId { get; set; }
    }
}
