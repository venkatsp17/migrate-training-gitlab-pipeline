using ShoppingAppAPI.Models;
using ShoppingAppAPI.Models.DTO_s;
using ShoppingAppAPI.Models.DTO_s.Order_DTO_s;
using ShoppingAppAPI.Models.DTO_s.Product_DTO_s;

namespace ShoppingAppAPI.Services.Interfaces
{
    public interface IProductServices
    {
        Task<IEnumerable<CustomerGetProductDTO>> GetProductsByName(string productName);

        Task<SellerGetProductDTO> AddProduct(AddProductDTO productDTO);

        Task<SellerGetProductDTO> UpdateProductPrice(decimal NewPrice, int ProductID);

        Task<SellerGetProductDTO> UpdateProductStock(int NewStock, int ProductID);

        Task<IEnumerable<CustomerGetProductDTO>> GetAllProducts(int page, int pageSize, string query);

        Task<PaginatedResult<SellerGetProductDTO>> ViewAllSellerProducts(int SellerID, int offset, int limit, string searchQuery);

        Task<IEnumerable<TopSellingProductDTO>> ViewAllTopSellingSellerProducts(int SellerID);

    }
}
