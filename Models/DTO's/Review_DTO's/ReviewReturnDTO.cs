namespace ShoppingAppAPI.Models.DTO_s.Review_DTO_s
{
    public class ReviewReturnDTO
    {
        public int ReviewID { get; set; }
        public int ProductID { get; set; }
        public int CustomerID { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime Review_Date { get; set; }
    }
}
