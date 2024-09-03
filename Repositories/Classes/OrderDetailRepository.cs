using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Interfaces;

namespace ShoppingAppAPI.Repositories.Classes
{
    /// <summary>
    /// Repository class for managing order details in the shopping application.
    /// </summary>
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly ShoppingAppContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderDetailRepository"/> class.
        /// </summary>
        /// <param name="context">The database context for the shopping application.</param>
        public OrderDetailRepository(ShoppingAppContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new order detail to the database.
        /// </summary>
        /// <param name="item">The order detail to add.</param>
        /// <returns>The added order detail.</returns>
        public async Task<OrderDetail> Add(OrderDetail item)
        {
            _context.OrderDetails.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// Deletes an order detail from the database.
        /// </summary>
        /// <param name="key">The ID of the order detail to delete.</param>
        /// <returns>The deleted order detail.</returns>
        public async Task<OrderDetail> Delete(int key)
        {
            var item = await Get(key);
            _context.OrderDetails.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// Gets all order details from the database.
        /// </summary>
        /// <returns>A list of all order details.</returns>
        /// <exception cref="NoAvailableItemException">Thrown when no order details are available.</exception>
        public async Task<IEnumerable<OrderDetail>> Get()
        {
            return await _context.OrderDetails.ToListAsync() ?? throw new NoAvailableItemException("Order Details");
        }

        /// <summary>
        /// Updates an existing order detail in the database.
        /// </summary>
        /// <param name="item">The order detail to update.</param>
        /// <returns>The updated order detail.</returns>
        public async Task<OrderDetail> Update(OrderDetail item)
        {
            _context.OrderDetails.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// Gets a specific order detail by its ID.
        /// </summary>
        /// <param name="key">The ID of the order detail to retrieve.</param>
        /// <returns>The order detail with the specified ID.</returns>
        /// <exception cref="NotFoundException">Thrown when the order detail is not found.</exception>
        public async Task<OrderDetail> Get(int key)
        {
            return await _context.OrderDetails.FirstOrDefaultAsync(c => c.OrderDetailID == key) ?? throw new NotFoundException("OrderDetail");
        }

        /// <summary>
        /// Gets order details for a specific seller by the seller's ID.
        /// </summary>
        /// <param name="SellerID">The ID of the seller whose order details to retrieve.</param>
        /// <returns>A list of order details for the specified seller.</returns>
        public async Task<IEnumerable<OrderDetail>> GetSellerOrderDetails(int SellerID)
        {
            return await _context.OrderDetails
                .Include(od => od.Order).ThenInclude(o => o.Customer).Include(od => od.Product)
                .Where(od => od.SellerID == SellerID)
                .ToListAsync();
        }


        public async Task<IEnumerable<OrderDetail>> GetSellerTopProducts(int SellerID)
        {
            return await _context.OrderDetails
                .Include(od => od.Order).Include(od => od.Product).ThenInclude(p => p.Category)
                .Where(od => od.SellerID == SellerID)
                .ToListAsync();
        }
    }
}
