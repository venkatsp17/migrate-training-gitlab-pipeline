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
    public class CartItemRepositoryTests
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
        public async Task Add_CartItem_ShouldAddCartItem()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new CartItemRepository(context);
            var cartItem = new CartItem { ProductID = 1, CartID = 1, Quantity = 2, Price = 10.99 };

            // Act
            var result = await repository.Add(cartItem);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.ProductID, Is.EqualTo(cartItem.ProductID));
        }

        [Test]
        public async Task Update_CartItem_ShouldUpdateCartItem()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new CartItemRepository(context);
            var cartItem = new CartItem { ProductID = 1, CartID = 1, Quantity = 2, Price = 10.99 };
            context.CartItems.Add(cartItem);
            await context.SaveChangesAsync();

            // Act
            cartItem.Quantity = 3;
            var result = await repository.Update(cartItem);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Quantity, Is.EqualTo(3));
        }

        [Test]
        public async Task Delete_CartItem_ShouldDeleteCartItem()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new CartItemRepository(context);
            var cartItem = new CartItem { ProductID = 1, CartID = 1, Quantity = 2, Price = 10.99 };
            context.CartItems.Add(cartItem);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.Delete(cartItem.CartItemID);

            // Assert
            Assert.NotNull(result);
            Assert.ThrowsAsync<NotFoundException>(async () => await repository.Get(cartItem.CartItemID));
        }

        [Test]
        public async Task Get_CartItem_ShouldReturnCartItem()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new CartItemRepository(context);
            var cartItem = new CartItem { ProductID = 1, CartID = 1, Quantity = 2, Price = 10.99 };
            context.CartItems.Add(cartItem);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.Get(cartItem.CartItemID);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.ProductID, Is.EqualTo(cartItem.ProductID));
        }

        [Test]
        public async Task Get_CartItem_ShouldThrowNotFoundException()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new CartItemRepository(context);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(async () => await repository.Get(999));
        }

        [Test]
        public async Task GetAll_CartItems_ShouldReturnAllCartItems()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new CartItemRepository(context);
            context.CartItems.AddRange(
                new CartItem { ProductID = 1, CartID = 1, Quantity = 2, Price = 10.99 },
                new CartItem { ProductID = 2, CartID = 1, Quantity = 3, Price = 15.99 });
            await context.SaveChangesAsync();

            // Act
            var result = await repository.Get();

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count(), Is.EqualTo(2));
        }
    }
}
