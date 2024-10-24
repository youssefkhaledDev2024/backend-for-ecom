using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.DAL.Entities
{
    public class Categories : IEntity
    {
      public int ID { get; set; }
        [Required]
       public string CategoryName { get; set; }

        public ICollection<Products> Products { get; set; } = new HashSet<Products>();
    }
}
