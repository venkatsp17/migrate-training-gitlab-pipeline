using ShoppingAppAPI.Models;
using ShoppingAppAPI.Models.DTO_s;

namespace ShoppingAppAPI.Repositories.Interfaces
{
    public interface IRegistrationRepository
    {
        public Task<(Customer customer, User user)> AddCustomer_UserTransaction(UserRegisterDTO userRegisterDTO);
        public Task<(Seller seller, User user)> AddSeller_UserTransaction(UserRegisterDTO userRegisterDTO);
    }
}
