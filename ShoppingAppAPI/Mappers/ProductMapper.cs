using ShoppingAppAPI.Models.DTO_s.Seller_DTO_s;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Models.DTO_s.Product_DTO_s;
using ShoppingAppAPI.Models.DTO_s;
using ShoppingAppAPI.Models.DTO_s.Review_DTO_s;

namespace ShoppingAppAPI.Mappers
{
    public class ProductMapper
    {
        public static SellerGetProductDTO MapToSellerProductDTO(Product product)
        {
            return new SellerGetProductDTO
            {
                ProductID = product.ProductID,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryID = product.CategoryID,
                Brand = product.Brand,
                Image_URL = product.Image_URL,
                Stock_Quantity = product.Stock_Quantity,
                Creation_Date = product.Creation_Date,
                Last_Updated = product.Last_Updated,
            };
        }

        public static CustomerGetProductDTO MapToCustomerProductDTO(Product product)
        {
            return new CustomerGetProductDTO
            {
                ProductID = product.ProductID,
                Name = product.Name,
                Stock = product.Stock_Quantity,
                Description = product.Description,
                Price = product.Price,
                CategoryID = product.CategoryID,
                Brand = product.Brand,
                Image_URL = product.Image_URL,
                Seller = SellerMapper.MapToSellerDTO(product.Seller),
                Reviews = product.Reviews == null ? new List<ReviewReturnDTO>() : product.Reviews.Select(r => new ReviewReturnDTO
                {
                    ReviewID = r.ReviewID,
                    ProductID = r.ProductID,
                    Comment = r.Comment,
                    CustomerID = r.CustomerID,
                    Rating = r.Rating,
                    Review_Date = r.Review_Date,
                }).ToList(),
            };
        }

        public static CustomerGetProductDTO1 MapToCustomerProductDTO1(Product product)
        {
            return new CustomerGetProductDTO1
            {
                ProductID = product.ProductID,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryID = product.CategoryID,
                Brand = product.Brand,
                Image_URL = product.Image_URL,
            };
        }
    }
}
