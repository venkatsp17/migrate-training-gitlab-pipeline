using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingAppAPI.Models
{
    public class Seller
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SellerID { get; set; }
        [Required]
        public int UserID { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Phone_Number { get; set; }
        public DateTime? Date_of_Birth { get; set; }
        public string? Gender { get; set; }
        public string? Profile_Picture_URL { get; set; }
        [Required]
        public string Account_Status { get; set; }
        public DateTime? Last_Login { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}