using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Models;
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
    public class CategoryServicesTests
    {
        private ICategoryRepository _categoryRepository;
        private IProductRepository _productRepository;
        private ICategoryServices _categoryServices;

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
            var _context = GetInMemoryDbContext();
            _categoryRepository = new CategoryRepository(_context);
            _productRepository = new ProductRepository(_context);
            IList<Review> reviews = new List<Review>();
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
            var product = new Product { Brand = "Dell", Description = "", Image_URL = "", Name = "Laptop", Seller = Seller, Reviews = reviews, CategoryID =1 };
            var product1 = new Product { Brand = "ABB", Description = "", Image_URL = "", Name = "Puma Shoe", Seller = Seller, Reviews = reviews, CategoryID = 1 };

            // Seed the database with test data
            var category = new Category
            {
                CategoryID = 1,
                Name = "Electronics",
                
            };
            _productRepository.Add(product);
            _productRepository.Add(product1);
            _categoryRepository.Add(category);
            _categoryServices = new CategoryServices(_categoryRepository);
        }

        [Test]
        public async Task GetProductsByCategory_CategoryExists_ReturnsProducts()
        {
            // Act
            var result = await _categoryServices.GetProductsByCategory("Electronics");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Name, Is.EqualTo("Laptop"));
        }

        [Test]
        public void GetProductsByCategory_CategoryDoesNotExist_ThrowsException()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () => await _categoryServices.GetProductsByCategory("NonExistentCategory"));
            Assert.That(ex.Message, Is.EqualTo("Unable to get category at this moment: Category with given ID Not Found!"));
        }
    }
}
