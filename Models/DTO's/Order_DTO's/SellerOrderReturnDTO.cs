using static ShoppingAppAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;
using ShoppingAppAPI.Models.DTO_s.Customer_DTO_s;
using ShoppingAppAPI.Models.DTO_s.OrderDetail_DTO;

namespace ShoppingAppAPI.Models.DTO_s.Order_DTO_s
{
    public class SellerOrderReturnDTO
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public DateTime Order_Date { get; set; }
        public OrderStatus Status { get; set; }
        public string Address { get; set; }
        public decimal Total_Amount { get; set; }
        public string Shipping_Method { get; set; }
        public decimal Shipping_Cost { get; set; }
        public DateTime Last_Updated { get; set; }
        public CustomerDTO Customer { get; set; }
        public ICollection<ReturnOrderDetailDTO> OrderDetails { get; set; }
    }
}
