using ShoppingAppAPI.Models.DTO_s;

namespace ShoppingAppAPI.Services.Interfaces
{
    public interface ICategoryServices
    {
        Task<IEnumerable<CustomerGetProductDTO>> GetProductsByCategory(string Name);
    }
}
