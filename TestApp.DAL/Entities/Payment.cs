using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.DAL.Entities
{
    public class Payment
    {
        public int ID { get; set; }
        public DateTime PaymentDate { get; set; }

        public decimal Amount { get; set; }

        public string UserId { get; set; } // FK to ApplicationUser
        public virtual Users User { get; set; } // Navigation property

        public virtual ICollection<PaymentDetail> PaymentDetails { get; set; }
    }
}
