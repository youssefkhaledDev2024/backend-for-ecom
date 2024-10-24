namespace TestApp.ModelClasses
{
    public class ProductsModel
    {
        public int ID { get; set; }
        public string ProductTittle { get; set; }

        public decimal ProductPrice { get; set; }

        public string ProductDescription { get; set; }

        public double Rating { get; set; }

        public int Count { get; set; }

        public string? ImageName { get; set; }

        public int? CategoryId { get; set; }

       
    }
}
