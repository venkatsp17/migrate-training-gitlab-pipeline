using ShoppingAppAPI.Models;

namespace ShoppingAppAPI.Repositories.Interfaces
{
    public interface IOrderRepository : IRepository<int, Order>
    {
        Task<IEnumerable<Order>> GetCustomerOrders(int CustomerID);
    }
}
