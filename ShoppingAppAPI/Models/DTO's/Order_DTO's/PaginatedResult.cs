namespace ShoppingAppAPI.Models.DTO_s.Order_DTO_s
{
    public class PaginatedResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
