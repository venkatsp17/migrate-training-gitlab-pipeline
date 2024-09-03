using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Interfaces;

namespace ShoppingAppAPI.Repositories.Classes
{
    /// <summary>
    /// Repository class for managing customers in the shopping application.
    /// </summary>
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ShoppingAppContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerRepository"/> class.
        /// </summary>
        /// <param name="context">The database context for the shopping application.</param>
        public CustomerRepository(ShoppingAppContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new customer to the database.
        /// </summary>
        /// <param name="item">The customer to add.</param>
        /// <returns>The added customer.</returns>
        public async Task<Customer> Add(Customer item)
        {
            _context.Customers.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// Deletes a customer from the database.
        /// </summary>
        /// <param name="key">The ID of the customer to delete.</param>
        /// <returns>The deleted customer.</returns>
        public async Task<Customer> Delete(int key)
        {
            var item = await Get(key);
            _context.Customers.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// Gets all customers from the database.
        /// </summary>
        /// <returns>A list of all customers.</returns>
        /// <exception cref="NoAvailableItemException">Thrown when no customers are available.</exception>
        public async Task<IEnumerable<Customer>> Get()
        {
            return await _context.Customers.ToListAsync() ?? throw new NoAvailableItemException("Customers");
        }

        /// <summary>
        /// Updates an existing customer in the database.
        /// </summary>
        /// <param name="item">The customer to update.</param>
        /// <returns>The updated customer.</returns>
        public async Task<Customer> Update(Customer item)
        {
            _context.Customers.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// Gets a specific customer by its ID.
        /// </summary>
        /// <param name="key">The ID of the customer to retrieve.</param>
        /// <returns>The customer with the specified ID.</returns>
        /// <exception cref="NotFoundException">Thrown when the customer is not found.</exception>
        public async Task<Customer> Get(int key)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.CustomerID == key) ?? throw new NotFoundException("Customer");
        }

        /// <summary>
        /// Gets a customer by their email.
        /// </summary>
        /// <param name="email">The email of the customer to retrieve.</param>
        /// <returns>The customer associated with the specified email.</returns>
        /// <exception cref="NotFoundException">Thrown when the customer is not found.</exception>
        public async Task<Customer> GetCustomerByEmail(string email)
        {
            var customer = await _context.Customers
                 .Include(c => c.Cart)
                 .Include(c => c.Orders)
                 .FirstOrDefaultAsync(c => c.Email == email);
            return customer;
        }

        public async Task<Customer> GetCustomerByUserID(int key)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.UserID == key) ?? throw new NotFoundException("Customer");
        }
    }
}
