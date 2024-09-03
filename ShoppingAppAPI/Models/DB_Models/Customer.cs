using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingAppAPI.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerID { get; set; }
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
        public ICollection<Order> Orders { get; set; }

        public ICollection<Review> Reviews { get; set; }
        public Cart Cart { get; set; }
    }
}