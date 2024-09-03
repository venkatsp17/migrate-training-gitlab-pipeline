using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Models.DTO_s;
using ShoppingAppAPI.Repositories.Interfaces;

namespace ShoppingAppAPI.Repositories.Classes
{
    /// <summary>
    /// Repository class for managing user registrations in the shopping application.
    /// </summary>
    public class RegistrationRepository : IRegistrationRepository
    {
        private readonly ShoppingAppContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationRepository"/> class.
        /// </summary>
        /// <param name="context">The database context for the shopping application.</param>
        public RegistrationRepository(ShoppingAppContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Registers a new customer and user within a transaction.
        /// </summary>
        /// <param name="userRegisterDTO">The DTO containing user registration details.</param>
        /// <returns>A tuple containing the registered customer and user.</returns>
        /// <exception cref="UnableToRegisterException">Thrown when there is an error registering the user.</exception>
        public async Task<(Customer customer, User user)> AddCustomer_UserTransaction(UserRegisterDTO userRegisterDTO)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    User user = new User
                    {
                        Username = userRegisterDTO.Username,
                        Password = userRegisterDTO.Password,
                        Password_Hashkey = userRegisterDTO.Password_Hashkey,
                        IsAdmin = userRegisterDTO.IsAdmin,
                        Role = userRegisterDTO.Role
                    };
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    Customer customer = new Customer
                    {
                        UserID = user.UserID,
                        Email = userRegisterDTO.Email,
                        Name = userRegisterDTO.Name,
                        Address = userRegisterDTO.Address,
                        Phone_Number = userRegisterDTO.Phone_Number,
                        Date_of_Birth = userRegisterDTO.Date_of_Birth,
                        Gender = userRegisterDTO.Gender,
                        Profile_Picture_URL = userRegisterDTO.Profile_Picture_URL,
                        Account_Status = "InActive"
                    };

                    _context.Customers.Add(customer);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return (customer, user);
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw new UnableToRegisterException("Error Registering User!");
                }
            }
        }

        /// <summary>
        /// Registers a new seller and user within a transaction.
        /// </summary>
        /// <param name="userRegisterDTO">The DTO containing user registration details.</param>
        /// <returns>A tuple containing the registered seller and user.</returns>
        /// <exception cref="UnableToRegisterException">Thrown when there is an error registering the user.</exception>
        public async Task<(Seller seller, User user)> AddSeller_UserTransaction(UserRegisterDTO userRegisterDTO)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    User user = new User
                    {
                        Username = userRegisterDTO.Username,
                        Password = userRegisterDTO.Password,
                        Password_Hashkey = userRegisterDTO.Password_Hashkey,
                        IsAdmin = userRegisterDTO.IsAdmin,
                        Role = userRegisterDTO.Role
                    };
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    Seller seller = new Seller
                    {
                        UserID = user.UserID,
                        Email = userRegisterDTO.Email,
                        Name = userRegisterDTO.Name,
                        Address = userRegisterDTO.Address,
                        Phone_Number = userRegisterDTO.Phone_Number,
                        Date_of_Birth = userRegisterDTO.Date_of_Birth,
                        Gender = userRegisterDTO.Gender,
                        Profile_Picture_URL = userRegisterDTO.Profile_Picture_URL,
                        Account_Status = "InActive"
                    };

                    _context.Sellers.Add(seller);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return (seller, user);
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw new UnableToRegisterException("Error Registering User!");
                }
            }
        }
    }
}
