using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models.DTO_s;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Classes;
using ShoppingAppAPI.Repositories.Interfaces;
using ShoppingAppAPI.Services.Classes;
using ShoppingAppAPI.Services.Interfaces;

namespace ShoppingAppTest.Service_Tests
{
    [TestFixture]
    public class SellerServicesTests
    {
        private ISellerRepository _sellerRepository;
        private ISellerServices _sellerServices;

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
            _sellerRepository = new SellerRepository(_context);
            _sellerServices = new SellerServices(_sellerRepository);

            // Seed the database with test data
            var seller = new Seller
            {
                SellerID = 1,
                Name = "John Doe",
                Email = "john.doe@example.com",
                Address = "123 Main St",
                Phone_Number = "123-456-7890",
                Profile_Picture_URL = "http://example.com/profile.jpg",
                Last_Login = DateTime.Now,
                Account_Status = "Active"

            };

            _context.Sellers.Add(seller);
            _context.SaveChanges();
        }

        [Test]
        public async Task UpdateSellerLastLogin_UpdatesLastLogin()
        {
            // Arrange
            int sellerID = 1;

            // Act
            var result = await _sellerServices.UpdateSellerLastLogin(sellerID);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Last_Login, Is.EqualTo(DateTime.Now).Within(1).Minutes);
        }

        [Test]
        public async Task UpdateSeller_UpdatesSellerDetails()
        {
            // Arrange
            var updateDTO = new SellerUpdateDTO
            {
                SellerID = 1,
                Name = "Jane Doe",
                Email = "jane.doe@example.com",
                Address = "456 Main St",
                Phone_Number = "987-654-3210",
                Profile_Picture_URL = "http://example.com/profile_new.jpg"
            };

            // Act
            var result = await _sellerServices.UpdateSeller(updateDTO);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Name, Is.EqualTo(updateDTO.Name));
                Assert.That(result.Email, Is.EqualTo(updateDTO.Email));
                Assert.That(result.Address, Is.EqualTo(updateDTO.Address));
                Assert.That(result.Phone_Number, Is.EqualTo(updateDTO.Phone_Number));
                Assert.That(result.Profile_Picture_URL, Is.EqualTo(updateDTO.Profile_Picture_URL));
            });
        }
    }
}
