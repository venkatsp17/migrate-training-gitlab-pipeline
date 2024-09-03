using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Interfaces;

namespace ShoppingAppAPI.Repositories.Classes
{
    /// <summary>
    /// Repository class for managing reviews in the shopping application.
    /// </summary>
    public class ReviewRepository : IRepository<int, Review>
    {
        private readonly ShoppingAppContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReviewRepository"/> class.
        /// </summary>
        /// <param name="context">The database context for the shopping application.</param>
        public ReviewRepository(ShoppingAppContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new review to the database.
        /// </summary>
        /// <param name="item">The review to add.</param>
        /// <returns>The added review.</returns>
        public async Task<Review> Add(Review item)
        {
            _context.Reviews.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// Deletes a review from the database.
        /// </summary>
        /// <param name="key">The ID of the review to delete.</param>
        /// <returns>The deleted review.</returns>
        public async Task<Review> Delete(int key)
        {
            var item = await Get(key);
            _context.Reviews.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// Gets all reviews from the database.
        /// </summary>
        /// <returns>A list of all reviews.</returns>
        /// <exception cref="NoAvailableItemException">Thrown when no reviews are available.</exception>
        public async Task<IEnumerable<Review>> Get()
        {
            return await _context.Reviews.ToListAsync() ?? throw new NoAvailableItemException("Reviews");
        }

        /// <summary>
        /// Updates an existing review in the database.
        /// </summary>
        /// <param name="item">The review to update.</param>
        /// <returns>The updated review.</returns>
        public async Task<Review> Update(Review item)
        {
            _context.Reviews.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// Gets a specific review by its ID.
        /// </summary>
        /// <param name="key">The ID of the review to retrieve.</param>
        /// <returns>The review with the specified ID.</returns>
        /// <exception cref="NotFoundException">Thrown when the review is not found.</exception>
        public async Task<Review> Get(int key)
        {
            return await _context.Reviews.FirstOrDefaultAsync(c => c.ReviewID == key) ?? throw new NotFoundException("Review");
        }
    }
}
