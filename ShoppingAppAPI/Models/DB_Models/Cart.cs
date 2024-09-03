using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static ShoppingAppAPI.Models.Enums;

namespace ShoppingAppAPI.Models
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartID { get; set; }
        [Required]
        public int CustomerID { get; set; }
        public CartStatus Cart_Status { get; set; }
        [Required]
        public DateTime Last_Updated { get; set; }

        public Customer Customer { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}