using ShoppingAppAPI.Models;

namespace ShoppingAppAPI.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<int,  Product>
    {
        Task<Product> GetProductByName(string productName);

        Task<IEnumerable<Product>> GetAllProductsAsync();
    }
}
