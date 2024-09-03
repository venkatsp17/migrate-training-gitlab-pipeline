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
    public class SellerRepositoryTests
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
        public async Task Add_Seller_ShouldAddSeller()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new SellerRepository(context);
            var seller = new Seller
            {
                UserID = 1,
                Email = "seller@example.com",
                Name = "John Doe",
                Address = "123 Main St",
                Phone_Number = "123-456-7890",
                Account_Status = "Active"
            };

            // Act
            var result = await repository.Add(seller);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Email, Is.EqualTo(seller.Email));
        }

        [Test]
        public async Task Update_Seller_ShouldUpdateSeller()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new SellerRepository(context);
            var seller = new Seller
            {
                UserID = 1,
                Email = "seller@example.com",
                Name = "John Doe",
                Address = "123 Main St",
                Phone_Number = "123-456-7890",
                Account_Status = "Active"
            };
            context.Sellers.Add(seller);
            await context.SaveChangesAsync();

            // Act
            seller.Name = "Jane Doe";
            var result = await repository.Update(seller);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Name, Is.EqualTo("Jane Doe"));
        }

        [Test]
        public async Task Delete_Seller_ShouldDeleteSeller()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new SellerRepository(context);
            var seller = new Seller
            {
                UserID = 1,
                Email = "seller@example.com",
                Name = "John Doe",
                Address = "123 Main St",
                Phone_Number = "123-456-7890",
                Account_Status = "Active"
            };
            context.Sellers.Add(seller);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.Delete(seller.SellerID);

            // Assert
            Assert.NotNull(result);
            Assert.ThrowsAsync<NotFoundException>(async () => await repository.Get(seller.SellerID));
        }

        [Test]
        public async Task Get_Seller_ShouldReturnSeller()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new SellerRepository(context);
            var seller = new Seller
            {
                UserID = 1,
                Email = "seller@example.com",
                Name = "John Doe",
                Address = "123 Main St",
                Phone_Number = "123-456-7890",
                Account_Status = "Active"
            };
            context.Sellers.Add(seller);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.Get(seller.SellerID);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Email, Is.EqualTo(seller.Email));
        }

        [Test]
        public async Task GetAll_Seller_ShouldReturnSeller()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new SellerRepository(context);
            var seller = new Seller
            {
                UserID = 1,
                Email = "seller@example.com",
                Name = "John Doe",
                Address = "123 Main St",
                Phone_Number = "123-456-7890",
                Account_Status = "Active"
            };
            var seller1 = new Seller
            {
                UserID = 2,
                Email = "seller@example.com",
                Name = "John Doe",
                Address = "123 Main St",
                Phone_Number = "123-456-7890",
                Account_Status = "Active"
            };
            context.Sellers.Add(seller);
            context.Sellers.Add(seller1);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.Get();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task Get_Seller_ShouldThrowNotFoundException()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new SellerRepository(context);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(async () => await repository.Get(999));
        }

        [Test]
        public async Task GetSellerByEmail_ShouldReturnSeller()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new SellerRepository(context);
            var seller = new Seller
            {
                UserID = 1,
                Email = "seller@example.com",
                Name = "John Doe",
                Address = "123 Main St",
                Phone_Number = "123-456-7890",
                Account_Status = "Active"
            };
            context.Sellers.Add(seller);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetSellerByEmail(seller.Email);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Email, Is.EqualTo(seller.Email));
        }
    }
}
