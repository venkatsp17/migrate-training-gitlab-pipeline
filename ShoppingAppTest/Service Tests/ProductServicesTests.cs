using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Models.DTO_s;
using ShoppingAppAPI.Repositories.Classes;
using ShoppingAppAPI.Repositories.Interfaces;
using ShoppingAppAPI.Services.Classes;
using ShoppingAppAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingAppTest.Service_Tests
{
    [TestFixture]
    public class ProductServicesTests
    {
        private ShoppingAppContext _context;
        private IProductRepository _productRepository;
        private IProductServices _productServices;

        private ShoppingAppContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ShoppingAppContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new ShoppingAppContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        [SetUp]
        public void Setup()
        {
            _context = GetInMemoryDbContext();
            _productRepository = new ProductRepository(_context);

            var Seller = new Seller()
            {
                SellerID = 1,
                UserID = 123,
                Email = "seller@example.com",
                Name = "John Doe",
                Address = "123 Main St",
                Phone_Number = "123-456-7890",
                Date_of_Birth = new DateTime(1990, 5, 15),
                Gender = "Male",
                Profile_Picture_URL = "https://example.com/profile.jpg",
                Account_Status = "Active",
                Last_Login = DateTime.Now,
                Products = new List<Product>(), // You can add products here if needed
                OrderDetails = new List<OrderDetail>() // You can add order details here if needed
            };
            // Seed the database with test data
            var product = new Product
            {
                ProductID = 1,
                Name = "Test Product",
                SellerID = 1,
                Description = "Test Description",
                Price = 100.0m,
                CategoryID = 1,
                Brand = "Test Brand",
                Image_URL = "http://example.com/image.jpg",
                Stock_Quantity = 10,
                Creation_Date = DateTime.Now,
                Last_Updated = DateTime.Now,
                Seller = Seller
            };
            _productRepository.Add(product);
            _productServices = new ProductServices(_productRepository);
        }

        [Test]
        public async Task AddProduct_ValidProduct_ReturnsSellerGetProductDTO()
        {
            // Arrange
            var productDTO = new AddProductDTO
            {
                Name = "New Product",
                SellerID = 1,
                Description = "New Product Description",
                Price = 50.0m,
                CategoryID = 1,
                Brand = "New Brand",
                Image_URL = "http://example.com/newimage.jpg",
                Stock_Quantity = 20,
            };

            // Act
            var result = await _productServices.AddProduct(productDTO);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(productDTO.Name));
            Assert.That(result.Price, Is.EqualTo(productDTO.Price));
        }

        [Test]
        public void AddProduct_ExistingProduct_ThrowsItemAlreadyExistException()
        {
            // Arrange
            var productDTO = new AddProductDTO
            {
                Name = "Test Product",
                SellerID = 1,
                Description = "Duplicate Product Description",
                Price = 50.0m,
                CategoryID = 1,
                Brand = "Duplicate Brand",
                Image_URL = "http://example.com/duplicateimage.jpg",
                Stock_Quantity = 20,
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<UnableToAddItemException>(async () => await _productServices.AddProduct(productDTO));
            Assert.That(ex.Message, Is.EqualTo("Product already exist!"));
        }

        [Test]
        public async Task GetProductsByName_ValidName_ReturnsProducts()
        {
            // Arrange
            var productName = "Test";

            // Act
            var result = await _productServices.GetProductsByName(productName);

            // Assert
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.First().Name, Does.Contain(productName));
        }

        [Test]
        public async Task GetProductsByName_InvalidName_ThrowsNoAvailableItemException()
        {
            // Arrange
            var productName = "Nonexistent Product";

            // Act & Assert
            var ex = await _productServices.GetProductsByName(productName);
            Assert.That(ex.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task UpdateProductPrice_ValidProduct_ReturnsUpdatedProduct()
        {
            // Arrange
            var newPrice = 150.0m;
            var productID = 1;

            // Act
            var result = await _productServices.UpdateProductPrice(newPrice, productID);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Price, Is.EqualTo(newPrice));
        }

        [Test]
        public void UpdateProductPrice_InvalidProduct_ThrowsNoAvailableItemException()
        {
            // Arrange
            var newPrice = 150.0m;
            var productID = 999; // Non-existing ProductID

            // Act & Assert
            var ex = Assert.ThrowsAsync<UnableToUpdateItemException>(async () => await _productServices.UpdateProductPrice(newPrice, productID));
            Assert.That(ex.Message, Is.EqualTo("Product with given ID Not Found!"));
        }

        [Test]
        public async Task UpdateProductStock_ValidProduct_ReturnsUpdatedProduct()
        {
            // Arrange
            var newStock = 30;
            var productID = 1;

            // Act
            var result = await _productServices.UpdateProductStock(newStock, productID);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Stock_Quantity, Is.EqualTo(newStock));
        }

        [Test]
        public void UpdateProductStock_InvalidProduct_ThrowsNoAvailableItemException()
        {
            // Arrange
            var newStock = 30;
            var productID = 999; // Non-existing ProductID

            // Act & Assert
            var ex = Assert.ThrowsAsync<UnableToUpdateItemException>(async () => await _productServices.UpdateProductStock(newStock, productID));
            Assert.That(ex.Message, Is.EqualTo("Product with given ID Not Found!"));
        }
    }
}
