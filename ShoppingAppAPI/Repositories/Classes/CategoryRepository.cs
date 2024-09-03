using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Interfaces;

namespace ShoppingAppAPI.Repositories.Classes
{
    /// <summary>
    /// Repository class for managing categories in the shopping application.
    /// </summary>
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ShoppingAppContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryRepository"/> class.
        /// </summary>
        /// <param name="context">The database context for the shopping application.</param>
        public CategoryRepository(ShoppingAppContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new category to the database.
        /// </summary>
        /// <param name="item">The category to add.</param>
        /// <returns>The added category.</returns>
        public async Task<Category> Add(Category item)
        {
            _context.Categories.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// Deletes a category from the database.
        /// </summary>
        /// <param name="key">The ID of the category to delete.</param>
        /// <returns>The deleted category.</returns>
        public async Task<Category> Delete(int key)
        {
            var item = await Get(key);
            _context.Categories.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// Gets all categories from the database.
        /// </summary>
        /// <returns>A list of all categories.</returns>
        /// <exception cref="NoAvailableItemException">Thrown when no categories are available.</exception>
        public async Task<IEnumerable<Category>> Get()
        {
            return await _context.Categories.ToListAsync() ?? throw new NoAvailableItemException("Categories");
        }

        /// <summary>
        /// Updates an existing category in the database.
        /// </summary>
        /// <param name="item">The category to update.</param>
        /// <returns>The updated category.</returns>
        public async Task<Category> Update(Category item)
        {
            _context.Categories.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// Gets a specific category by its name.
        /// </summary>
        /// <param name="Name">The name of the category to retrieve.</param>
        /// <returns>The category with the specified name.</returns>
        /// <exception cref="NotFoundException">Thrown when the category is not found.</exception>
        public async Task<Category> GetCategoryByName(string Name)
        {
            return await _context.Categories.Include(c => c.Products).ThenInclude(p => p.Seller).FirstOrDefaultAsync(c => c.Name == Name) ?? throw new NotFoundException("Category");
        }

        /// <summary>
        /// Gets a specific category by its ID.
        /// </summary>
        /// <param name="key">The ID of the category to retrieve.</param>
        /// <returns>The category with the specified ID.</returns>
        /// <exception cref="NotFoundException">Thrown when the category is not found.</exception>
        public async Task<Category> Get(int key)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.CategoryID == key) ?? throw new NotFoundException("Category");
        }
    }
}
