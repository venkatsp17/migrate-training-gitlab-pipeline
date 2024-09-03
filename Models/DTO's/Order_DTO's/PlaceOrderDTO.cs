using ShoppingAppAPI.Models.DTO_s.Customer_DTO_s;
using ShoppingAppAPI.Models.DTO_s.OrderDetail_DTO;
using System.ComponentModel.DataAnnotations;
using static ShoppingAppAPI.Models.Enums;

namespace ShoppingAppAPI.Models.DTO_s.Order_DTO_s
{
    public class PlaceOrderDTO
    {
        [Required(ErrorMessage = "CustomerID is required")]
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Shipping_Method is required")]
        public string Shipping_Method { get; set; }

        [Required(ErrorMessage = "Shipping_Cost is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Shipping_Cost must be greater than zero")]
        public decimal Shipping_Cost { get; set; }
    }

    public class OrderDetailDTO
    {
        [Required(ErrorMessage = "ProductID is required")]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Unit_Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Unit_Price must be greater than zero")]
        public decimal Unit_Price { get; set; }
    }
}
