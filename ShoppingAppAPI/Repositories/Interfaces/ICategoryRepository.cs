using ShoppingAppAPI.Models;

namespace ShoppingAppAPI.Repositories.Interfaces
{
    public interface ICategoryRepository : IRepository<int, Category>
    {
        Task<Category> GetCategoryByName(string Name);
    }
}
