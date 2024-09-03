using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models.DTO_s;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingAppAPI.Repositories.Interfaces;
using System.Security.Cryptography;
using Microsoft.Data.Sqlite;

namespace ShoppingAppTest.RepositoryTests
{
    [TestFixture]
    public class RegistrationRepositoryTests
    {
        IRegistrationRepository _registrationRepository;
        private ShoppingAppContext GetInMemoryDbContext()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ShoppingAppContext>().UseSqlite(connection).Options;
            var context = new ShoppingAppContext(options);
            context.Database.EnsureCreated();
            return context;
            //using (var context = new ShoppingAppContext(options))
            //{
            //    context.Employees.Add(new Employee { Id = 1, FirstName = "John", LastName = "Doe", Address = "123 Street", HomePhone = "111-111-1111", CellPhone = "222-222-2222" });
            //    context.SaveChanges();
            //}
        }
        private UserRegisterDTO MapUserRegisterDTO(RegisterDTO registerDTO)
        {
            UserRegisterDTO user = new UserRegisterDTO();
            HMACSHA512 hMACSHA = new HMACSHA512();
            user.Password_Hashkey = hMACSHA.Key;
            user.Username = registerDTO.Username;
            user.IsAdmin = false;
            user.Password = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password));
            user.Address = registerDTO.Address;
            user.Email = registerDTO.Email;
            user.Date_of_Birth = registerDTO.Date_of_Birth;
            user.Phone_Number = registerDTO.Phone_Number;
            user.Gender = registerDTO.Gender;
            user.Profile_Picture_URL = registerDTO.Profile_Picture_URL;
            user.Name = registerDTO.Name;
            return user;
        }

        [Test]
        public async Task AddCustomer_UserTransaction_ShouldAddCustomerAndUser()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            _registrationRepository = new RegistrationRepository(context);
            var userRegisterDTO = new RegisterDTO
            {
                Username = "testuser",
                Password = "123456",
                Email = "customer@example.com",
                Name = "John Doe",
                Address = "123 Main St",
                Phone_Number = "123-456-7890",
                Date_of_Birth = new DateTime(1990, 1, 1),
                Gender = "Male",
                Profile_Picture_URL = "http://example.com/profile.jpg"
            };
            UserRegisterDTO userRepostioryRegisterDTO = MapUserRegisterDTO(userRegisterDTO);
            userRepostioryRegisterDTO.Role = Enums.UserRole.Customer;
            // Act
            var result = await _registrationRepository.AddCustomer_UserTransaction(userRepostioryRegisterDTO);

            // Assert
            Assert.NotNull(result.customer);
            Assert.NotNull(result.user);
            Assert.That(result.user.Username, Is.EqualTo(userRegisterDTO.Username));
            Assert.That(result.customer.Email, Is.EqualTo(userRegisterDTO.Email));
        }

        [Test]
        public async Task AddSeller_UserTransaction_ShouldAddSellerAndUser()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new RegistrationRepository(context);
            var userRegisterDTO = new RegisterDTO
            {
                Username = "testuser",
                Password = "123456",
                Email = "customer@example.com",
                Name = "John Doe",
                Address = "123 Main St",
                Phone_Number = "123-456-7890",
                Date_of_Birth = new DateTime(1990, 1, 1),
                Gender = "Male",
                Profile_Picture_URL = "http://example.com/profile.jpg"
            };
            UserRegisterDTO userRepostioryRegisterDTO = MapUserRegisterDTO(userRegisterDTO);
            userRepostioryRegisterDTO.Role = Enums.UserRole.Seller;

            // Act
            var result = await repository.AddSeller_UserTransaction(userRepostioryRegisterDTO);

            // Assert
            Assert.NotNull(result.seller);
            Assert.NotNull(result.user);
            Assert.That(result.user.Username, Is.EqualTo(userRegisterDTO.Username));
            Assert.That(result.seller.Email, Is.EqualTo(userRegisterDTO.Email));
        }

        [Test]
        public void AddCustomer_UserTransaction_ShouldRollbackOnError()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new RegistrationRepository(context);
            var userRegisterDTO = new RegisterDTO
            {
                Username = "testuser",
                Password = "123456",
                Email = "customer@example.com",
                Address = "123 Main St",
                Phone_Number = "123-456-7890",
                Date_of_Birth = new DateTime(1990, 1, 1),
                Gender = "Male",
                Profile_Picture_URL = "http://example.com/profile.jpg"
            };
            UserRegisterDTO userRepostioryRegisterDTO = MapUserRegisterDTO(userRegisterDTO);
            userRepostioryRegisterDTO.Role = Enums.UserRole.Customer;

            // Act & Assert
            Assert.ThrowsAsync<UnableToRegisterException>(async () =>
            {
                var result = await repository.AddCustomer_UserTransaction(userRepostioryRegisterDTO);
            });
        }

        [Test]
        public void AddSeller_UserTransaction_ShouldRollbackOnError()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new RegistrationRepository(context);
            var userRegisterDTO = new RegisterDTO
            {
                Username = "testuser",
                Password = "123456",
                Email = "customer@example.com",
                Address = "123 Main St",
                Phone_Number = "123-456-7890",
                Date_of_Birth = new DateTime(1990, 1, 1),
                Gender = "Male",
                Profile_Picture_URL = "http://example.com/profile.jpg"
            };
            UserRegisterDTO userRepostioryRegisterDTO = MapUserRegisterDTO(userRegisterDTO);
            userRepostioryRegisterDTO.Role = Enums.UserRole.Seller;

            // Act & Assert
            Assert.ThrowsAsync<UnableToRegisterException>(async () =>
            {
               var result = await repository.AddSeller_UserTransaction(userRepostioryRegisterDTO);

            });
        }
    }
}
