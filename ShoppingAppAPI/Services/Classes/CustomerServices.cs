using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Mappers;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Models.DTO_s;
using ShoppingAppAPI.Models.DTO_s.Customer_DTO_s;
using ShoppingAppAPI.Repositories.Interfaces;
using ShoppingAppAPI.Services.Interfaces;

namespace ShoppingAppAPI.Services.Classes
{
    public class CustomerServices : ICustomerServices
    {
        private readonly ICustomerRepository _customerRepository;
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerServices"/> class.
        /// </summary>
        /// <param name="customerRepository">The repository for managing customers.</param>
        public CustomerServices(ICustomerRepository customerRepository) { 
            _customerRepository = customerRepository;
        }
        /// <summary>
        /// Updates the last login time for a customer.
        /// </summary>
        /// <param name="CustomerID">The ID of the customer.</param>
        /// <returns>The updated customer object.</returns>
        public async Task<Customer> UpdateCustomerLastLogin(int CustomerID)
        {
           Customer customer = await _customerRepository.Get(CustomerID);
           customer.Last_Login = DateTime.Now;
           return await _customerRepository.Update(customer);
        }

        /// <summary>
        /// Updates customer information.
        /// </summary>
        /// <param name="updateDTO">The DTO containing updated customer information.</param>
        /// <returns>The updated customer DTO.</returns>
        public async Task<CustomerDTO> UpdateCustomer(CustomerUpdateDTO updateDTO)
        {
            try
            {
                Customer customer = await _customerRepository.Get(updateDTO.CustomerID);
                customer.Phone_Number = updateDTO.Phone_Number;
                customer.Address = updateDTO.Address;
                customer.Email = updateDTO.Email;
                customer.Name = updateDTO.Name;
                customer.Profile_Picture_URL = string.IsNullOrEmpty(updateDTO.Profile_Picture_URL)?customer.Profile_Picture_URL: updateDTO.Profile_Picture_URL;
                Customer updatedCustomer = await _customerRepository.Update(customer);
                if (updatedCustomer == null)
                {
                    throw new UnableToUpdateItemException("Unable to Update Profile at this moment!");
                }
                return CustomerMapper.MapToCustomerDTO(updatedCustomer);
            }
            catch (Exception ex)
            {
                throw new UnableToUpdateItemException(ex.Message);
            }
        }

        public async Task<CustomerDTO> GetCustomerProfile(int UserID)
        {
            try
            {
                Customer customer = await _customerRepository.GetCustomerByUserID(UserID);
                return CustomerMapper.MapToCustomerDTO(customer);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
