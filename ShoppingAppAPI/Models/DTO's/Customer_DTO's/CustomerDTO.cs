using System.ComponentModel.DataAnnotations;

namespace ShoppingAppAPI.Models.DTO_s.Customer_DTO_s
{
    public class CustomerDTO
    {
        public int CustomerID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone_Number { get; set; }
        public DateTime? Date_of_Birth { get; set; }
        public string? Gender { get; set; }
        public string? Profile_Picture_URL { get; set; }
    }
}
