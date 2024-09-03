using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Interfaces;

namespace ShoppingAppAPI.Repositories.Classes
{
    /// <summary>
    /// Repository class for managing products in the shopping application.
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        private readonly ShoppingAppContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductRepository"/> class.
        /// </summary>
        /// <param name="context">The database context for the shopping application.</param>
        public ProductRepository(ShoppingAppContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new product to the database.
        /// </summary>
        /// <param name="item">The product to add.</param>
        /// <returns>The added product.</returns>
        public async Task<Product> Add(Product item)
        {
            _context.Products.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// Deletes a product from the database.
        /// </summary>
        /// <param name="key">The ID of the product to delete.</param>
        /// <returns>The deleted product.</returns>
        public async Task<Product> Delete(int key)
        {
            var item = await Get(key);
            _context.Products.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// Updates an existing product in the database.
        /// </summary>
        /// <param name="item">The product to update.</param>
        /// <returns>The updated product.</returns>
        public async Task<Product> Update(Product item)
        {
            _context.Products.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// Gets a specific product by its ID.
        /// </summary>
        /// <param name="key">The ID of the product to retrieve.</param>
        /// <returns>The product with the specified ID.</returns>
        /// <exception cref="NotFoundException">Thrown when the product is not found.</exception>
        public async Task<Product> Get(int key)
        {
            return await _context.Products.Include(p => p.Seller).Include(p => p.Reviews).FirstOrDefaultAsync(c => c.ProductID == key) ?? throw new NotFoundException("Product");
        }

        /// <summary>
        /// Gets all products from the database.
        /// </summary>
        /// <returns>A list of all products.</returns>
        public async Task<IEnumerable<Product>> Get()
        {
            return await _context.Products.Include(p => p.Seller).Include(p => p.Reviews).ToListAsync();
        }

        /// <summary>
        /// Gets a specific product by its name.
        /// </summary>
        /// <param name="productName">The name of the product to retrieve.</param>
        /// <returns>The product with the specified name.</returns>
        public async Task<Product> GetProductByName(string productName)
        {
            return await _context.Products.Include(p => p.Seller).Include(p => p.Reviews).FirstOrDefaultAsync(c => c.Name == productName);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Reviews)
                .Include(p => p.Seller)
                .ToListAsync();
        }
    }
}
