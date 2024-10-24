using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.DAL.Entities
{
    public class Products : IEntity
    {
        public int ID { get; set; }

        public string ProductTittle { get; set; }

        public decimal ProductPrice { get; set; }

        public string ProductDescription { get; set; }

         public double Rating {  get; set; }

        public int Count { get; set; }

        public string? ImageName { get; set; }

        public int? CategoryId { get; set; }

        public Categories Category { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

        public double CalculateAverageRating()
        {
            if (Ratings.Any())
            {
                Rating = Ratings.Average(r => r.Score);
            }
            else
            {
                Rating = 0; // Default to 0 if no ratings
            }
            return Rating;
        }



    }
}
