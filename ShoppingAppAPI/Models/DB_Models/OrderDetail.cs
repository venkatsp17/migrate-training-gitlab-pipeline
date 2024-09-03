using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShoppingAppAPI.Models
{
    public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderDetailID { get; set; }
        [Required]
        public int OrderID { get; set; }
        [Required]
        public int SellerID { get; set; }
        [Required]
        public int ProductID { get; set; }
        [Required]
        public int Quantity { get; set; }
        //[Required]
        //public decimal Unit_Price { get; set; } // Old column
        [Required]
        public decimal Price { get; set; } // New column
        [Required]
        public string Size { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
        public Seller Seller { get; set; }
    }
}