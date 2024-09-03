using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShoppingAppAPI.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductID { get; set; }
        [Required]
        public int SellerID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int CategoryID { get; set; }
        public string Brand { get; set; }
        [Required]
        public string Image_URL { get; set; }
        [Required]
        public int Stock_Quantity { get; set; }
        public DateTime Creation_Date { get; set; }
        [Required]
        public DateTime Last_Updated { get; set; }
        public Seller Seller { get; set; }
        public Category Category { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        //public ICollection<CartItem> CartItems { get; set; }
    }
}
