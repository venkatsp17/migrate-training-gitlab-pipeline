using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Interfaces;

namespace ShoppingAppAPI.Repositories.Classes
{
    /// <summary>
    /// Repository class for managing payments in the shopping application.
    /// </summary>
    public class PaymentRepository : IRepository<int, Payment>
    {
        private readonly ShoppingAppContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentRepository"/> class.
        /// </summary>
        /// <param name="context">The database context for the shopping application.</param>
        public PaymentRepository(ShoppingAppContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new payment to the database.
        /// </summary>
        /// <param name="item">The payment to add.</param>
        /// <returns>The added payment.</returns>
        public async Task<Payment> Add(Payment item)
        {
            _context.Payments.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// Deletes a payment from the database.
        /// </summary>
        /// <param name="key">The ID of the payment to delete.</param>
        /// <returns>The deleted payment.</returns>
        public async Task<Payment> Delete(int key)
        {
            var item = await Get(key);
            _context.Payments.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// Gets all payments from the database.
        /// </summary>
        /// <returns>A list of all payments.</returns>
        /// <exception cref="NoAvailableItemException">Thrown when no payments are available.</exception>
        public async Task<IEnumerable<Payment>> Get()
        {
            return await _context.Payments.ToListAsync() ?? throw new NoAvailableItemException("Payments");
        }

        /// <summary>
        /// Updates an existing payment in the database.
        /// </summary>
        /// <param name="item">The payment to update.</param>
        /// <returns>The updated payment.</returns>
        public async Task<Payment> Update(Payment item)
        {
            _context.Payments.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// Gets a specific payment by its ID.
        /// </summary>
        /// <param name="key">The ID of the payment to retrieve.</param>
        /// <returns>The payment with the specified ID.</returns>
        /// <exception cref="NotFoundException">Thrown when the payment is not found.</exception>
        public async Task<Payment> Get(int key)
        {
            return await _context.Payments.FirstOrDefaultAsync(c => c.PaymentID == key) ?? throw new NotFoundException("Payment");
        }
    }
}
