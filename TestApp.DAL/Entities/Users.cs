using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.DAL.Entities
{
    public class Users : IdentityUser
    {
        public string FullName { get; set; }
        public virtual ICollection<Products> Products { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
