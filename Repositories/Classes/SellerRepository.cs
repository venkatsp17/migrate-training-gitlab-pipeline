using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Interfaces;

namespace ShoppingAppAPI.Repositories.Classes
{
    /// <summary>
    /// Repository class for managing sellers in the shopping application.
    /// </summary>
    public class SellerRepository : ISellerRepository
    {
        private readonly ShoppingAppContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SellerRepository"/> class.
        /// </summary>
        /// <param name="context">The database context for the shopping application.</param>
        public SellerRepository(ShoppingAppContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new seller to the database.
        /// </summary>
        /// <param name="item">The seller to add.</param>
        /// <returns>The added seller.</returns>
        public async Task<Seller> Add(Seller item)
        {
            _context.Sellers.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// Deletes a seller from the database.
        /// </summary>
        /// <param name="key">The ID of the seller to delete.</param>
        /// <returns>The deleted seller.</returns>
        public async Task<Seller> Delete(int key)
        {
            var item = await Get(key);
            _context.Sellers.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// Gets all sellers from the database.
        /// </summary>
        /// <returns>A list of all sellers.</returns>
        /// <exception cref="NoAvailableItemException">Thrown when no sellers are available.</exception>
        public async Task<IEnumerable<Seller>> Get()
        {
            return await _context.Sellers.ToListAsync() ?? throw new NoAvailableItemException("Sellers");
        }

        /// <summary>
        /// Updates an existing seller in the database.
        /// </summary>
        /// <param name="item">The seller to update.</param>
        /// <returns>The updated seller.</returns>
        public async Task<Seller> Update(Seller item)
        {
            _context.Sellers.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// Gets a specific seller by its ID.
        /// </summary>
        /// <param name="key">The ID of the seller to retrieve.</param>
        /// <returns>The seller with the specified ID.</returns>
        /// <exception cref="NotFoundException">Thrown when the seller is not found.</exception>
        public async Task<Seller> Get(int key)
        {
            return await _context.Sellers.FirstOrDefaultAsync(c => c.SellerID == key) ?? throw new NotFoundException("Seller");
        }

        /// <summary>
        /// Gets a specific seller by its email address.
        /// </summary>
        /// <param name="email">The email address of the seller to retrieve.</param>
        /// <returns>The seller with the specified email address.</returns>
        public async Task<Seller> GetSellerByEmail(string email)
        {
            return await _context.Sellers.FirstOrDefaultAsync(s => s.Email == email);
        }

        public async Task<Seller> GetSellerByUserID(int userID)
        {
            return await _context.Sellers.FirstOrDefaultAsync(s => s.UserID == userID);
        }
    }
}
