using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Mappers;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Models.DTO_s;
using ShoppingAppAPI.Models.DTO_s.Order_DTO_s;
using ShoppingAppAPI.Models.DTO_s.Product_DTO_s;
using ShoppingAppAPI.Repositories.Classes;
using ShoppingAppAPI.Repositories.Interfaces;
using ShoppingAppAPI.Services.Interfaces;
using static ShoppingAppAPI.Models.Enums;

namespace ShoppingAppAPI.Services.Classes
{
    public class ProductServices : IProductServices
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        /// <summary>
        /// Constructor for ProductServices class.
        /// </summary>
        /// <param name="productRepository">Product repository dependency.</param>
        public ProductServices(IProductRepository productRepository, IOrderDetailRepository orderDetailRepository)
        {
            _productRepository = productRepository;
            _orderDetailRepository = orderDetailRepository;
        }
        /// <summary>
        /// Adds a new product.
        /// </summary>
        /// <param name="productDTO">Product information DTO.</param>
        /// <returns>Returns the added product DTO.</returns>
        /// <exception cref="ItemAlreadyExistException">Thrown when product already exist</exception>
        /// <exception cref="UnableToAddItemException">Thrown when unable to add item to database</exception>
        public async Task<SellerGetProductDTO> AddProduct(AddProductDTO productDTO)
        {
            try
            {
                Product existingProduct = await _productRepository.GetProductByName(productDTO.Name);
                if (existingProduct != null)
                {
                    throw new ItemAlreadyExistException("Product");
                }
                Product product = new Product()
                {
                    Name = productDTO.Name,
                    SellerID = productDTO.SellerID,
                    Description = productDTO.Description,
                    Price = productDTO.Price,
                    CategoryID = productDTO.CategoryID,
                    Brand  = productDTO.Brand,
                    Image_URL = productDTO.Image_URL,
                    Stock_Quantity = productDTO.Stock_Quantity,
                    Creation_Date = DateTime.Now,
                    Last_Updated = DateTime.Now,
                };
                Product returnProduct = await _productRepository.Add(product);
                if(returnProduct == null) { 
                    throw new UnableToAddItemException("Unable to Add Product at this moment!");
                }
                SellerGetProductDTO sellerGetProductDTO = ProductMapper.MapToSellerProductDTO(returnProduct);
                return sellerGetProductDTO;
            }
            catch (Exception ex)
            {
                throw new UnableToAddItemException(ex.Message);
            }
        }
        /// <summary>
        /// Retrieves products by name.
        /// </summary>
        /// <param name="productName">Name of the product to search.</param>
        /// <returns>Returns a list of products matching the specified name.</returns>
        /// <exception cref="NoAvailableItemException">Thrown when no items are available</exception>
        public async Task<IEnumerable<CustomerGetProductDTO>> GetProductsByName(string productName)
        {
            try
            {
                IEnumerable<Product> products = await _productRepository.Get();
                if (products.Count() == 0)
                {
                    throw new NoAvailableItemException("Products");
                }
                return products.Where(p => p.Name.Contains(productName, System.StringComparison.OrdinalIgnoreCase)).Select(p => ProductMapper.MapToCustomerProductDTO(p));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
        /// <summary>
        /// Updates the price of a product.
        /// </summary>
        /// <param name="NewPrice">New price value.</param>
        /// <param name="ProductID">ID of the product to update.</param>
        /// <returns>Returns the updated product DTO.</returns>
        /// <exception cref="NoAvailableItemException">Thrown when no items are available</exception>
        /// <exception cref="UnableToUpdateItemException">Thrown when unable to update item in database</exception>
        public async Task<SellerGetProductDTO> UpdateProductPrice(decimal NewPrice, int ProductID)
        {
            try
            {
                Product product = await _productRepository.Get(ProductID);
                if (product == null)
                {
                    throw new NoAvailableItemException("Product");
                }
                product.Price = NewPrice;
                product.Last_Updated = DateTime.Now;
                Product updatedProduct = await _productRepository.Update(product);
                if (updatedProduct == null)
                {
                    throw new UnableToUpdateItemException("Unable to Update Product at this moment!");
                }
                SellerGetProductDTO sellerGetProductDTO = ProductMapper.MapToSellerProductDTO(updatedProduct);
                return sellerGetProductDTO;
            }
            catch (Exception ex)
            {
                throw new UnableToUpdateItemException(ex.Message);
            }
        }
        /// <summary>
        /// Updates the stock quantity of a product.
        /// </summary>
        /// <param name="NewStock">New stock quantity.</param>
        /// <param name="ProductID">ID of the product to update.</param>
        /// <returns>Returns the updated product DTO.</returns>
        /// <exception cref="NoAvailableItemException">Thrown when no items are available</exception>
        /// <exception cref="UnableToUpdateItemException">Thrown when unable to update item in database</exception>
        public async Task<SellerGetProductDTO> UpdateProductStock(int NewStock, int ProductID)
        {
            try
            {
                Product product = await _productRepository.Get(ProductID);
                if (product == null)
                {
                    throw new NoAvailableItemException("Product");
                }
                product.Stock_Quantity = NewStock;
                product.Last_Updated = DateTime.Now;
                Product updatedProduct = await _productRepository.Update(product);
                if (updatedProduct == null)
                {
                    throw new UnableToUpdateItemException("Unable to Update Product at this moment!");
                }
                SellerGetProductDTO sellerGetProductDTO = ProductMapper.MapToSellerProductDTO(updatedProduct);
                return sellerGetProductDTO;
            }
            catch (Exception ex)
            {
                throw new UnableToUpdateItemException(ex.Message);
            }
        }



        public async Task<IEnumerable<CustomerGetProductDTO>> GetAllProducts(int page, int pageSize, string query)
        {
            try
            {
                if(query == "null")
                {
                    query = null;
                }
                var products = await _productRepository.GetAllProductsAsync();
                var filteredProducts = products
                  .Where(p => string.IsNullOrEmpty(query) || p.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
                  .ToList();
                var paginated = filteredProducts
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize);
                return paginated.Select(p => ProductMapper.MapToCustomerProductDTO(p));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        public async Task<PaginatedResult<SellerGetProductDTO>> ViewAllSellerProducts(int SellerID, int offset, int limit, string searchQuery)
        {
            try
            {
                IEnumerable<Product> products = await _productRepository.Get();
                if (!products.Any())
                {
                    throw new NoAvailableItemException("Orders");
                }
                if (searchQuery == "null")
                {
                    searchQuery = null;
                }
                var filteredProducts = products
                    .Where(p => (p.SellerID == SellerID) && (string.IsNullOrEmpty(searchQuery) || p.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)))
                    .ToList();

                var paginatedProducts = filteredProducts
                    .Skip(offset)
                    .Take(limit)
                    .OrderBy(p => p.ProductID)
                    .ToList();

                var totalCount = filteredProducts.Count();

                var result = new PaginatedResult<SellerGetProductDTO>
                {
                    Items = paginatedProducts.Select(o => ProductMapper.MapToSellerProductDTO(o)).ToList(),
                    TotalCount = totalCount
                };

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<IEnumerable<TopSellingProductDTO>> ViewAllTopSellingSellerProducts(int SellerID)
        {
            try
            {
                var products = await _orderDetailRepository.GetSellerTopProducts(SellerID);
                if (!products.Any())
                {
                    throw new NoAvailableItemException("Products");
                }
                var topSellingProducts = products
                                    .GroupBy(od => new { od.ProductID, ProductName = od.Product.Name, CategoryName = od.Product.Category.Name })
                                    .Select(g => new TopSellingProductDTO
                                     {
                                          ProductID = g.Key.ProductID,
                                          ProductName = g.Key.ProductName,
                                          CategoryName = g.Key.CategoryName,
                                          QuantitySold = g.Sum(od => od.Quantity),
                                          TotalRevenue = g.Sum(od => od.Price * od.Quantity)
                                     })
                                     .OrderByDescending(dto => dto.QuantitySold)
                                     .ToList();
               return topSellingProducts;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
