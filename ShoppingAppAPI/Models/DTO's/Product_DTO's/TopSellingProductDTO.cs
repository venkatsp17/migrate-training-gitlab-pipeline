namespace ShoppingAppAPI.Models.DTO_s.Product_DTO_s
{
    public class TopSellingProductDTO
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public int QuantitySold { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
