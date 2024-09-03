using System.ComponentModel.DataAnnotations;

namespace ShoppingAppAPI.Models.DTO_s.Order_DTO_s
{
    public class UpdateOrderDeliveryDetailsDTO
    {
        [Required(ErrorMessage = "OrderID is required")]
        public int OrderID { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Shipping_Method is required")]
        public string Shipping_Method { get; set; }
    }
}
