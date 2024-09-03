using ShoppingAppAPI.Models;

namespace ShoppingAppAPI.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<int, User>
    {
        Task<User> GetCustomerDetailByEmail(string email);

        Task<User> GetSellerDetailByEmail(string email);
    }
}
