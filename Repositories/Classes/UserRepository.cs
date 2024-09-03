using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Interfaces;

namespace ShoppingAppAPI.Repositories.Classes
{
    /// <summary>
    /// Repository class for managing users in the shopping application.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly ShoppingAppContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context">The database context for the shopping application.</param>
        public UserRepository(ShoppingAppContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        /// <param name="item">The user to add.</param>
        /// <returns>The added user.</returns>
        public async Task<User> Add(User item)
        {
            _context.Users.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// Deletes a user from the database.
        /// </summary>
        /// <param name="key">The ID of the user to delete.</param>
        /// <returns>The deleted user.</returns>
        public async Task<User> Delete(int key)
        {
            var item = await Get(key);
            _context.Users.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// Gets all users from the database.
        /// </summary>
        /// <returns>A list of all users.</returns>
        /// <exception cref="NoAvailableItemException">Thrown when no users are available.</exception>
        public async Task<IEnumerable<User>> Get()
        {
            return await _context.Users.ToListAsync() ?? throw new NoAvailableItemException("Users");
        }

        /// <summary>
        /// Updates an existing user in the database.
        /// </summary>
        /// <param name="item">The user to update.</param>
        /// <returns>The updated user.</returns>
        public async Task<User> Update(User item)
        {
            _context.Users.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// Gets a specific user by its ID.
        /// </summary>
        /// <param name="key">The ID of the user to retrieve.</param>
        /// <returns>The user with the specified ID.</returns>
        /// <exception cref="NotFoundException">Thrown when the user is not found.</exception>
        public async Task<User> Get(int key)
        {
            return await _context.Users.FirstOrDefaultAsync(c => c.UserID == key) ?? throw new NotFoundException("User");
        }

        /// <summary>
        /// Gets the details of a customer by their email address.
        /// </summary>
        /// <param name="email">The email address of the customer to retrieve.</param>
        /// <returns>The user with the specified email address and associated customer details.</returns>
        public async Task<User> GetCustomerDetailByEmail(string email)
        {
            var user = await _context.Users
                  .Include(u => u.Customer)
                  .FirstOrDefaultAsync(u => u.Customer.Email == email);
            return user;
        }

        /// <summary>
        /// Gets the details of a seller by their email address.
        /// </summary>
        /// <param name="email">The email address of the seller to retrieve.</param>
        /// <returns>The user with the specified email address and associated seller details.</returns>
        public async Task<User> GetSellerDetailByEmail(string email)
        {
            var user = await _context.Users
                  .Include(u => u.Seller)
                  .FirstOrDefaultAsync(u => u.Seller.Email == email);
            return user;
        }
    }
}
