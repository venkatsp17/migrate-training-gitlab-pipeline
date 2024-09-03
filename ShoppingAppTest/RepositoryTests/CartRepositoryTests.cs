using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Classes;
using System;

namespace ShoppingAppTest.RepositoryTests
{
    [TestFixture]
    public class CartRepositoryTests
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
        public async Task Add_Cart_ShouldAddCart()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new CartRepository(context);
            var cart = new Cart { CustomerID = 1, Cart_Status = Enums.CartStatus.Open, Last_Updated = DateTime.Now };

            // Act
            var result = await repository.Add(cart);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.CustomerID, Is.EqualTo(cart.CustomerID));
        }

        [Test]
        public async Task Update_Cart_ShouldUpdateCart()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new CartRepository(context);
            var cart = new Cart { CustomerID = 1, Cart_Status = Enums.CartStatus.Open, Last_Updated = DateTime.Now };
            context.Carts.Add(cart);
            await context.SaveChangesAsync();

            // Act
            cart.Cart_Status = Enums.CartStatus.Closed;
            var result = await repository.Update(cart);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(Enums.CartStatus.Closed, result.Cart_Status);
        }

        [Test]
        public async Task Delete_Cart_ShouldDeleteCart()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new CartRepository(context);
            var cart = new Cart { CustomerID = 1, Cart_Status = Enums.CartStatus.Open, Last_Updated = DateTime.Now };
            context.Carts.Add(cart);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.Delete(cart.CartID);

            // Assert
            Assert.NotNull(result);
            //Assert.ThrowsAsync<NotFoundException>(async () => await repository.Get(cart.CartID));
        }

        [Test]
        public async Task Get_Cart_ShouldReturnCart()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new CartRepository(context);
            var cart = new Cart { CustomerID = 1, Cart_Status = Enums.CartStatus.Open, Last_Updated = DateTime.Now };
            context.Carts.Add(cart);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.Get(cart.CartID);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.CustomerID, Is.EqualTo(cart.CustomerID));
        }

        [Test]
        public async Task GetAll_Carts_ShouldReturnAllCarts()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new CartRepository(context);
            context.Carts.AddRange(
                new Cart { CustomerID = 1, Cart_Status = Enums.CartStatus.Open, Last_Updated = DateTime.Now },
                new Cart { CustomerID = 2, Cart_Status = Enums.CartStatus.Closed, Last_Updated = DateTime.Now });
            await context.SaveChangesAsync();

            // Act
            var result = await repository.Get();

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetCartsByName_ShouldReturnAllCarts()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new CartRepository(context);
            context.Carts.AddRange(
                new Cart { CustomerID = 1, Cart_Status = Enums.CartStatus.Open, Last_Updated = DateTime.Now },
                new Cart { CustomerID = 2, Cart_Status = Enums.CartStatus.Closed, Last_Updated = DateTime.Now });
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetCartByCustomerID(1);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.CartID, Is.EqualTo(1));
        }
    }
}