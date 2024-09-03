using ShoppingAppAPI.Models;

namespace ShoppingAppAPI.Repositories.Interfaces
{
    public interface ICustomerRepository : IRepository<int, Customer>
    {
        Task<Customer> GetCustomerByEmail(string email);

        Task<Customer> GetCustomerByUserID(int UserID);
    }
}
