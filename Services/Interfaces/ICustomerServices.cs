using ShoppingAppAPI.Models;
using ShoppingAppAPI.Models.DTO_s;
using ShoppingAppAPI.Models.DTO_s.Customer_DTO_s;

namespace ShoppingAppAPI.Services.Interfaces
{
    public interface ICustomerServices
    {
        Task<Customer> UpdateCustomerLastLogin(int CustomerID);

        Task<CustomerDTO> UpdateCustomer(CustomerUpdateDTO updateDTO);

        Task<CustomerDTO> GetCustomerProfile(int UserID);

    }
}
