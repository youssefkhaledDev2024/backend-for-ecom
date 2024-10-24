using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.DAL.Entities
{
    public class Rating
    {
        public int Id { get; set; }
        public int Score { get; set; }
        [ForeignKey("ProductId")]

        public int ProductId { get; set; }
        public Products Product { get; set; }
        [ForeignKey("UserId")]

        public string UserId { get; set; } // FK to ApplicationUser
        public virtual Users User { get; set; } // Navigation property
    }
}
