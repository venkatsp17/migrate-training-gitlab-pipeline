using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Classes;
using ShoppingAppAPI.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingAppTest.RepositoryTests
{
    [TestFixture]
    public class UserRepositoryTests
    {
        private ShoppingAppContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ShoppingAppContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new ShoppingAppContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        [Test]
        public async Task Add_User_Success()
        {
            var context = GetInMemoryDbContext();   
            var _userRepository = new UserRepository(context);
            var newUser = new User { UserID = 3, Username = "User3", Password = Encoding.UTF8.GetBytes("password"), Password_Hashkey = Encoding.UTF8.GetBytes("passwordHash") };

            var result = await _userRepository.Add(newUser);

            Assert.NotNull(result);
            Assert.AreEqual(newUser.Username, result.Username);
        }
        [Test]
        public async Task Delete_User_Success()
        {
            var _context = GetInMemoryDbContext();
            var _userRepository = new UserRepository(_context);
            var newUser = new User { UserID = 3, Username = "User3", Password = Encoding.UTF8.GetBytes("password"), Password_Hashkey = Encoding.UTF8.GetBytes("passwordHash") };

            await _userRepository.Add(newUser);
            var userToDelete = _context.Users.First();

            var result = await _userRepository.Delete(userToDelete.UserID);

            Assert.NotNull(result);
            Assert.AreEqual(userToDelete.UserID, result.UserID);

            var userInDb = await _context.Users.FindAsync(userToDelete.UserID);
            Assert.Null(userInDb);
        }

        [Test]
        public async Task Update_User_Success()
        {
            var _context = GetInMemoryDbContext();
            var _userRepository = new UserRepository(_context);
            var newUser = new User { UserID = 3, Username = "User3", Password = Encoding.UTF8.GetBytes("password"), Password_Hashkey = Encoding.UTF8.GetBytes("passwordHash") };

            await _userRepository.Add(newUser);
            var userToUpdate = _context.Users.First();
            userToUpdate.Username = "UpdatedUser";

            var result = await _userRepository.Update(userToUpdate);

            Assert.NotNull(result);
            Assert.AreEqual("UpdatedUser", result.Username);

            var userInDb = await _context.Users.FindAsync(userToUpdate.UserID);
            Assert.NotNull(userInDb);
            Assert.AreEqual("UpdatedUser", userInDb.Username);
        }

        [Test]
        public async Task Get_AllUsers_Success()
        {
            var _context = GetInMemoryDbContext();
            var _userRepository = new UserRepository(_context);
            var newUser = new User { UserID = 3, Username = "User3", Password = Encoding.UTF8.GetBytes("password"), Password_Hashkey = Encoding.UTF8.GetBytes("passwordHash") };

            await _userRepository.Add(newUser);
            var result = await _userRepository.Get();

            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task GetUserById_ShouldReturnUser()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new UserRepository(context);
            var user = new User
            {
                Username = "testuser",
                Password = new byte[] { 1, 2, 3 },
                Password_Hashkey = new byte[] { 4, 5, 6 },
                IsAdmin = false,
                Role = Enums.UserRole.Customer
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.Get(user.UserID);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Username, Is.EqualTo(user.Username));
        }

        [Test]
        public void GetUserById_ShouldThrowNotFoundException()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new UserRepository(context);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(async () => await repository.Get(999));
        }

        [Test]
        public async Task GetCustomerDetailByEmail_ShouldReturnUser()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new UserRepository(context);
            var user = new User
            {
                Username = "testuser",
                Password = new byte[] { 1, 2, 3 },
                Password_Hashkey = new byte[] { 4, 5, 6 },
                IsAdmin = false,
                Role = Enums.UserRole.Customer,
                Customer = new Customer
                {
                    Email = "customer@example.com",
                    Name = "John Doe",
                    Address = "123 Main St",
                    Phone_Number = "123-456-7890",
                    Date_of_Birth = new DateTime(1990, 1, 1),
                    Gender = "Male",
                    Profile_Picture_URL = "http://example.com/profile.jpg",
                    Account_Status = "Active"
                }
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetCustomerDetailByEmail(user.Customer.Email);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Customer.Email, Is.EqualTo(user.Customer.Email));
        }

        [Test]
        public async Task GetSellerDetailByEmail_ShouldReturnUser()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new UserRepository(context);
            var user = new User
            {
                Username = "testseller",
                Password = new byte[] { 1, 2, 3 },
                Password_Hashkey = new byte[] { 4, 5, 6 },
                IsAdmin = false,
                Role = Enums.UserRole.Seller,
                Seller = new Seller
                {
                    Email = "seller@example.com",
                    Name = "Jane Doe",
                    Address = "456 Elm St",
                    Phone_Number = "987-654-3210",
                    Date_of_Birth = new DateTime(1985, 5, 15),
                    Gender = "Female",
                    Profile_Picture_URL = "http://example.com/profile.jpg",
                    Account_Status = "Active"
                }
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetSellerDetailByEmail(user.Seller.Email);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Seller.Email, Is.EqualTo(user.Seller.Email));
        }
    }
}
