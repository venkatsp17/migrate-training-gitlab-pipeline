using ShoppingAppAPI.Models.DTO_s;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Models.DTO_s.Order_DTO_s;

namespace ShoppingAppAPI.Mappers
{
    public class OrderMapper
    {
        public static SellerOrderReturnDTO MapToSellerOrderReturnDTO(Order order)
        {
            return new SellerOrderReturnDTO
            {
                OrderID = order.OrderID,
                CustomerID = order.CustomerID,
                Order_Date = order.Order_Date,
                Status = order.Status,
                Address = order.Address,
                Total_Amount = order.Total_Amount,
                Shipping_Method = order.Shipping_Method,
                Shipping_Cost = order.Shipping_Cost,
                Last_Updated = order.Last_Updated,
                Customer = CustomerMapper.MapToCustomerDTO(order.Customer),
                OrderDetails = order.OrderDetails.Select(od => OrderDetailMapper.MapToReturnOrderDetailDTO(od)).ToList(),
            };
        }

        public static CustomerOrderReturnDTO MapToCustomerOrderReturnDTO(Order order)
        {
            return new CustomerOrderReturnDTO
            {
                OrderID = order.OrderID,
                Order_Date = order.Order_Date,
                Status = order.Status,
                Address = order.Address,
                Total_Amount = order.Total_Amount,
                Shipping_Method = order.Shipping_Method,
                Shipping_Cost = order.Shipping_Cost,
                Last_Updated = order.Last_Updated,
                OrderDetails = order.OrderDetails.Select(od => OrderDetailMapper.MapToReturnOrderDetailDTO1(od)).ToList(),
            };
        }
    }
}
