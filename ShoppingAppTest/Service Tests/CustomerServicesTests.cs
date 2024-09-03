using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models.DTO_s;
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
    public class CustomerServicesTests
    {
        private ICustomerRepository _customerRepository;
        private ICustomerServices _customerServices;

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
            _customerRepository = new CustomerRepository(_context);
            _customerServices = new CustomerServices(_customerRepository);

            // Seed the database with test data
            var customer = new Customer
            {
                CustomerID = 1,
                Name = "John Doe",
                Email = "john.doe@example.com",
                Address = "123 Main St",
                Phone_Number = "123-456-7890",
                Profile_Picture_URL = "http://example.com/profile.jpg",
                Last_Login = DateTime.Now,
                Account_Status = "Active"
  
            };

            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        [Test]
        public async Task UpdateCustomerLastLogin_UpdatesLastLogin()
        {
            // Arrange
            int customerID = 1;

            // Act
            var result = await _customerServices.UpdateCustomerLastLogin(customerID);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Last_Login, Is.EqualTo(DateTime.Now).Within(1).Minutes);
        }

        [Test]
        public async Task UpdateCustomer_UpdatesCustomerDetails()
        {
            // Arrange
            var updateDTO = new CustomerUpdateDTO
            {
                CustomerID = 1,
                Name = "Jane Doe",
                Email = "jane.doe@example.com",
                Address = "456 Main St",
                Phone_Number = "987-654-3210",
                Profile_Picture_URL = "http://example.com/profile_new.jpg"
            };

            // Act
            var result = await _customerServices.UpdateCustomer(updateDTO);

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

        [Test]
        public void UpdateCustomer_CustomerDoesNotExist_ThrowsException()
        {
            // Arrange
            var updateDTO = new CustomerUpdateDTO
            {
                CustomerID = 999, // Non-existent ID
                Name = "Non Existent",
                Email = "non.existent@example.com",
                Address = "No Address",
                Phone_Number = "000-000-0000",
                Profile_Picture_URL = "http://example.com/non_existent.jpg"
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<UnableToUpdateItemException>(async () => await _customerServices.UpdateCustomer(updateDTO));
            Assert.That(ex.Message, Is.EqualTo("Customer with given ID Not Found!"));
        }
    }
}
