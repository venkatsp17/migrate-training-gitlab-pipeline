using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShoppingAppAPI.Models
{
    public class CartItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartItemID { get; set; }
        [Required]
        public int ProductID { get; set; }
        [Required]
        public int CartID { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public string Size { get; set; }
        public Product Product { get; set; }
        public Cart Cart { get; set; }
    }
}