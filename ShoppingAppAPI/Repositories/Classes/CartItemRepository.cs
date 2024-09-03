using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Interfaces;

namespace ShoppingAppAPI.Repositories.Classes
{
    /// <summary>
    /// Repository class for managing cart items in the shopping application.
    /// </summary>
    public class CartItemRepository : IRepository<int, CartItem>
    {
        private readonly ShoppingAppContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartItemRepository"/> class.
        /// </summary>
        /// <param name="context">The database context for the shopping application.</param>
        public CartItemRepository(ShoppingAppContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new cart item to the database.
        /// </summary>
        /// <param name="item">The cart item to add.</param>
        /// <returns>The added cart item.</returns>
        public async Task<CartItem> Add(CartItem item)
        {
            _context.CartItems.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// Deletes a cart item from the database.
        /// </summary>
        /// <param name="key">The ID of the cart item to delete.</param>
        /// <returns>The deleted cart item.</returns>
        public async Task<CartItem> Delete(int key)
        {
            var item = await Get(key);
            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// Gets all cart items from the database.
        /// </summary>
        /// <returns>A list of all cart items.</returns>
        /// <exception cref="NoAvailableItemException">Thrown when no cart items are available.</exception>
        public async Task<IEnumerable<CartItem>> Get()
        {
            return await _context.CartItems.ToListAsync() ?? throw new NoAvailableItemException("CartItems");
        }

        /// <summary>
        /// Updates an existing cart item in the database.
        /// </summary>
        /// <param name="item">The cart item to update.</param>
        /// <returns>The updated cart item.</returns>
        public async Task<CartItem> Update(CartItem item)
        {
            _context.CartItems.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// Gets a specific cart item by its ID.
        /// </summary>
        /// <param name="key">The ID of the cart item to retrieve.</param>
        /// <returns>The cart item with the specified ID.</returns>
        /// <exception cref="NotFoundException">Thrown when the cart item is not found.</exception>
        public async Task<CartItem> Get(int key)
        {
            return await _context.CartItems.FirstOrDefaultAsync(c => c.CartItemID == key) ?? throw new NotFoundException("CartItem");
        }
    }
}
