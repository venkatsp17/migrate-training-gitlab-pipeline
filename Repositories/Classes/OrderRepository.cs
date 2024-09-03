using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Interfaces;

namespace ShoppingAppAPI.Repositories.Classes
{
    /// <summary>
    /// Repository class for managing orders in the shopping application.
    /// </summary>
    public class OrderRepository : IOrderRepository
    {
        private readonly ShoppingAppContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderRepository"/> class.
        /// </summary>
        /// <param name="context">The database context for the shopping application.</param>
        public OrderRepository(ShoppingAppContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new order to the database.
        /// </summary>
        /// <param name="item">The order to add.</param>
        /// <returns>The added order.</returns>
        public async Task<Order> Add(Order item)
        {
            _context.Orders.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// Deletes an order from the database.
        /// </summary>
        /// <param name="key">The ID of the order to delete.</param>
        /// <returns>The deleted order.</returns>
        public async Task<Order> Delete(int key)
        {
            var item = await Get(key);
            _context.Orders.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// Updates an existing order in the database.
        /// </summary>
        /// <param name="item">The order to update.</param>
        /// <returns>The updated order.</returns>
        public async Task<Order> Update(Order item)
        {
            _context.Orders.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// Gets a specific order by its ID.
        /// </summary>
        /// <param name="key">The ID of the order to retrieve.</param>
        /// <returns>The order with the specified ID.</returns>
        /// <exception cref="NotFoundException">Thrown when the order is not found.</exception>
        public async Task<Order> Get(int key)
        {
            return await _context.Orders.Include(o => o.OrderDetails).ThenInclude(p => p.Product).Include(o => o.Customer).FirstOrDefaultAsync(c => c.OrderID == key) ?? throw new NotFoundException("Order");
        }

        /// <summary>
        /// Gets all orders from the database.
        /// </summary>
        /// <returns>A list of all orders.</returns>
        /// <exception cref="NoAvailableItemException">Thrown when no orders are available.</exception>
        public async Task<IEnumerable<Order>> Get()
        {
            return await _context.Orders.Include(o => o.OrderDetails).Include(o => o.Customer).ToListAsync() ?? throw new NoAvailableItemException("Orders");
        }

        public async Task<IEnumerable<Order>> GetCustomerOrders(int CustomerID)
        {
            return await _context.Orders.Include(o => o.OrderDetails).ThenInclude(p => p.Product).Include(o => o.Customer).Where(o=>o.CustomerID==CustomerID).ToListAsync();
        }
    }
}
