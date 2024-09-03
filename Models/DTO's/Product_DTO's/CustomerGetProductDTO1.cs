namespace ShoppingAppAPI.Models.DTO_s.Product_DTO_s
{
    public class CustomerGetProductDTO1
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryID { get; set; }
        public string Brand { get; set; }
        public string Image_URL { get; set; }
    }
}
