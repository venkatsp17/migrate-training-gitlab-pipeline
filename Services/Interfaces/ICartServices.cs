using ShoppingAppAPI.Models;
using ShoppingAppAPI.Models.DTO_s.Cart_DTO_s;
using ShoppingAppAPI.Models.DTO_s.Order_DTO_s;
using static ShoppingAppAPI.Models.Enums;

namespace ShoppingAppAPI.Services.Interfaces
{
    public interface ICartServices
    {
        Task<CartReturnDTO> AddItemToCart(CartItemGetDTO CartItem, int CustomerID);
        Task<CartReturnDTO> RemoveItemFromCart(int CartItemID);
        Task<CartReturnDTO> UpdateCartItemQuantity(int CartItemID, int Quantity);
        Task<CartReturnDTO1> GetCart(int CartID);
        Task<CartReturnDTO> CloseCart(int CartID);
    }
}
