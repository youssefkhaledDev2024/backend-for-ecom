using System.ComponentModel.DataAnnotations;

namespace TestApp.ModelClasses
{
    public class CategoryModel
    {
        public int ID { get; set; }
        [Required]
        public string CategoryName { get; set; }


    }
}
