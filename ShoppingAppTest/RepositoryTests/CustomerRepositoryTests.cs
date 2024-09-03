using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingAppTest.RepositoryTests
{
    [TestFixture]
    public class CustomerRepositoryTests
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
        public async Task Add_Customer_ShouldAddCustomer()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);
            var customer = new Customer
            {
                UserID = 1,
                Email = "test@example.com",
                Name = "John Doe",
                Address = "123 Main St",
                Phone_Number = "123-456-7890",
                Account_Status = "Active"
            };

            // Act
            var result = await repository.Add(customer);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Email, Is.EqualTo(customer.Email));
        }

        [Test]
        public async Task Update_Customer_ShouldUpdateCustomer()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);
            var customer = new Customer
            {
                UserID = 1,
                Email = "test@example.com",
                Name = "John Doe",
                Address = "123 Main St",
                Phone_Number = "123-456-7890",
                Account_Status = "Active"
            };
            context.Customers.Add(customer);
            await context.SaveChangesAsync();

            // Act
            customer.Phone_Number = "987-654-3210";
            var result = await repository.Update(customer);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Phone_Number, Is.EqualTo("987-654-3210"));
        }

        [Test]
        public async Task Delete_Customer_ShouldDeleteCustomer()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);
            var customer = new Customer
            {
                UserID = 1,
                Email = "test@example.com",
                Name = "John Doe",
                Address = "123 Main St",
                Phone_Number = "123-456-7890",
                Account_Status = "Active"
            };
            context.Customers.Add(customer);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.Delete(customer.CustomerID);

            // Assert
            Assert.NotNull(result);
            Assert.ThrowsAsync<NotFoundException>(async () => await repository.Get(customer.CustomerID));
        }

        [Test]
        public async Task Get_Customer_ShouldReturnCustomer()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);
            var customer = new Customer
            {
                UserID = 1,
                Email = "test@example.com",
                Name = "John Doe",
                Address = "123 Main St",
                Phone_Number = "123-456-7890",
                Account_Status = "Active"
            };
            context.Customers.Add(customer);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.Get(customer.CustomerID);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Email, Is.EqualTo(customer.Email));
        }

        [Test]
        public async Task Get_Customer_ShouldThrowNotFoundException()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(async () => await repository.Get(999));
        }

        [Test]
        public async Task GetCustomerByEmail_ShouldReturnCustomer()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);
            var customer = new Customer
            {
                UserID = 1,
                Email = "test@example.com",
                Name = "John Doe",
                Address = "123 Main St",
                Phone_Number = "123-456-7890",
                Account_Status = "Active"
            };
            context.Customers.Add(customer);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetCustomerByEmail(customer.Email);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Email, Is.EqualTo(customer.Email));
        }

        [Test]
        public async Task GetAll_Customers_ShouldReturnAllCustomers()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);
            context.Customers.AddRange(
                new Customer { Email = "test1@example.com", Name = "John Doe", Account_Status = "Active", Address = "TVL", Phone_Number = "54354453" },
                new Customer { Email = "test2@example.com", Name = "Jane Smith", Account_Status = "Active", Address = "TVL", Phone_Number = "54354453" });
            await context.SaveChangesAsync();

            // Act
            var result = await repository.Get();

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count(), Is.EqualTo(2));
        }
    }
}
