using ShoppingAppAPI.Models.DTO_s.Seller_DTO_s;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Models.DTO_s.Customer_DTO_s;

namespace ShoppingAppAPI.Mappers
{
    public class CustomerMapper
    {
        public static CustomerDTO MapToCustomerDTO(Customer customer)
        {
            return new CustomerDTO
            {
              CustomerID = customer.CustomerID,
              Email = customer.Email,
              Name = customer.Name,
              Address = customer.Address,
              Phone_Number = customer.Phone_Number,
              Date_of_Birth = customer.Date_of_Birth,
              Gender = customer.Gender,
              Profile_Picture_URL = customer.Profile_Picture_URL,
            };
        }
    }
}
