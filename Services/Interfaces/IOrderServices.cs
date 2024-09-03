using ShoppingAppAPI.Models;
using ShoppingAppAPI.Models.DTO_s.Order_DTO_s;
using static ShoppingAppAPI.Models.Enums;

namespace ShoppingAppAPI.Services.Interfaces
{
    public interface IOrderServices
    {
        Task<PaginatedResult<SellerOrderReturnDTO>> ViewAllSellerActiveOrders(int SellerID, int offset = 0, int limit = 10, string searchQuery = "");

        Task<IEnumerable<CustomerOrderReturnDTO>> ViewAllCustomerActiveOrders(int CustomerID);

        Task<SellerOrderReturnDTO> UpdateOrderStatus(OrderStatus orderStatus, int OrderID);

        Task<CustomerOrderReturnDTO> PlaceOrder(PlaceOrderDTO placeOrderDTO);
        Task<CustomerOrderReturnDTO> UpdateOrderDeliveryDetails(UpdateOrderDeliveryDetailsDTO updateOrderDeliveryDetailsDTO);

        Task<IEnumerable<CustomerOrderReturnDTO>> ViewCustomerOrderHistory(int CustomerID);

        Task<CustomerOrderReturnDTO> CustomerCancelOrder(int OrderID);
    }
}
