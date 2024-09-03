using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Interfaces;

namespace ShoppingAppAPI.Repositories.Classes
{
    /// <summary>
    /// Repository class for managing carts in the shopping application.
    /// </summary>
    public class CartRepository : ICartRepository
    {
        private readonly ShoppingAppContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartRepository"/> class.
        /// </summary>
        /// <param name="context">The database context for the shopping application.</param>
        public CartRepository(ShoppingAppContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new cart to the database.
        /// </summary>
        /// <param name="item">The cart to add.</param>
        /// <returns>The added cart.</returns>
        public async Task<Cart> Add(Cart item)
        {
            _context.Carts.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// Deletes a cart from the database.
        /// </summary>
        /// <param name="key">The ID of the cart to delete.</param>
        /// <returns>The deleted cart.</returns>
        public async Task<Cart> Delete(int key)
        {
            var item = await Get(key);
            _context.Carts.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// Gets all carts from the database.
        /// </summary>
        /// <returns>A list of all carts.</returns>
        /// <exception cref="NoAvailableItemException">Thrown when no carts are available.</exception>
        public async Task<IEnumerable<Cart>> Get()
        {
            return await _context.Carts.ToListAsync() ?? throw new NoAvailableItemException("Carts");
        }

        /// <summary>
        /// Updates an existing cart in the database.
        /// </summary>
        /// <param name="item">The cart to update.</param>
        /// <returns>The updated cart.</returns>
        public async Task<Cart> Update(Cart item)
        {
            _context.Carts.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// Gets a specific cart by its ID.
        /// </summary>
        /// <param name="key">The ID of the cart to retrieve.</param>
        /// <returns>The cart with the specified ID.</returns>
        public async Task<Cart> Get(int key)
        {
            return await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.CartID == key);
        }

        /// <summary>
        /// Gets a cart by the customer ID.
        /// </summary>
        /// <param name="customerID">The ID of the customer whose cart to retrieve.</param>
        /// <returns>The cart associated with the specified customer ID.</returns>
        public async Task<Cart> GetCartByCustomerID(int customerID)
        {
            return await _context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Product).FirstOrDefaultAsync(c => c.CustomerID == customerID);
        }
    }
}
