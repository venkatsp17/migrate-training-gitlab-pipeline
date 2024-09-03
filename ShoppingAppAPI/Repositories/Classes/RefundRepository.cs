using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Interfaces;

namespace ShoppingAppAPI.Repositories.Classes
{
    /// <summary>
    /// Repository class for managing refunds in the shopping application.
    /// </summary>
    public class RefundRepository : IRepository<int, Refund>
    {
        private readonly ShoppingAppContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="RefundRepository"/> class.
        /// </summary>
        /// <param name="context">The database context for the shopping application.</param>
        public RefundRepository(ShoppingAppContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new refund to the database.
        /// </summary>
        /// <param name="item">The refund to add.</param>
        /// <returns>The added refund.</returns>
        public async Task<Refund> Add(Refund item)
        {
            _context.Refunds.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// Deletes a refund from the database.
        /// </summary>
        /// <param name="key">The ID of the refund to delete.</param>
        /// <returns>The deleted refund.</returns>
        public async Task<Refund> Delete(int key)
        {
            var item = await Get(key);
            _context.Refunds.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// Gets all refunds from the database.
        /// </summary>
        /// <returns>A list of all refunds.</returns>
        /// <exception cref="NoAvailableItemException">Thrown when no refunds are available.</exception>
        public async Task<IEnumerable<Refund>> Get()
        {
            return await _context.Refunds.ToListAsync() ?? throw new NoAvailableItemException("Refunds");
        }

        /// <summary>
        /// Updates an existing refund in the database.
        /// </summary>
        /// <param name="item">The refund to update.</param>
        /// <returns>The updated refund.</returns>
        public async Task<Refund> Update(Refund item)
        {
            _context.Refunds.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// Gets a specific refund by its ID.
        /// </summary>
        /// <param name="key">The ID of the refund to retrieve.</param>
        /// <returns>The refund with the specified ID.</returns>
        /// <exception cref="NotFoundException">Thrown when the refund is not found.</exception>
        public async Task<Refund> Get(int key)
        {
            return await _context.Refunds.FirstOrDefaultAsync(c => c.RefundID == key) ?? throw new NotFoundException("Refund");
        }
    }
}
