using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Mappers;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Models.DTO_s;
using ShoppingAppAPI.Repositories.Interfaces;
using ShoppingAppAPI.Services.Interfaces;

namespace ShoppingAppAPI.Services.Classes
{
    public class CategoryServices : ICategoryServices
    {
        private readonly ICategoryRepository _categoryRepository;
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryServices"/> class.
        /// </summary>
        /// <param name="categoryRepository">The repository for managing categories.</param>
        public CategoryServices(ICategoryRepository categoryRepository) {
            _categoryRepository = categoryRepository;
        }

        /// <summary>
        /// Retrieves products belonging to a specific category.
        /// </summary>
        /// <param name="Name">The name of the category.</param>
        /// <returns>A collection of products in the specified category.</returns>
        public async Task<IEnumerable<CustomerGetProductDTO>> GetProductsByCategory(string Name)
        {
            try
            {
                Category category = await _categoryRepository.GetCategoryByName(Name);
                if (category == null)
                {
                    throw new NoAvailableItemException("Category");
                }
                return category.Products.Select(x => ProductMapper.MapToCustomerProductDTO(x));
            }
            catch(Exception exception) {
                throw new Exception("Unable to get category at this moment: "+exception.Message);
            }
           
           
        }
    }
}
