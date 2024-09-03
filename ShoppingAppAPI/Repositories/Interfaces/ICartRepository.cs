using ShoppingAppAPI.Models;

namespace ShoppingAppAPI.Repositories.Interfaces
{
    public interface ICartRepository : IRepository<int, Cart>
    {
        Task<Cart> GetCartByCustomerID(int customerID);
    }
}
