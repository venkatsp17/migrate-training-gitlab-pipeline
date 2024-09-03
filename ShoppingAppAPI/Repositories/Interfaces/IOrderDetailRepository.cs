using ShoppingAppAPI.Models;

namespace ShoppingAppAPI.Repositories.Interfaces
{
    public interface IOrderDetailRepository : IRepository<int, OrderDetail>
    {
        Task<IEnumerable<OrderDetail>> GetSellerOrderDetails(int SellerID);

        Task<IEnumerable<OrderDetail>> GetSellerTopProducts(int SellerID);
    }
}
