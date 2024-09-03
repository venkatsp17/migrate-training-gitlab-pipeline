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
    public class OrderDetailRepositoryTests
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
        public async Task Add_OrderDetail_ShouldAddOrderDetail()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new OrderDetailRepository(context);
            var orderDetail = new OrderDetail
            {
                OrderID = 1,
                SellerID =1,
                ProductID = 1,
                Quantity = 2,
                Price = 50.0m
            };

            // Act
            var result = await repository.Add(orderDetail);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.OrderID, Is.EqualTo(orderDetail.OrderID));
            Assert.That(result.ProductID, Is.EqualTo(orderDetail.ProductID));
        }

        [Test]
        public async Task Update_OrderDetail_ShouldUpdateOrderDetail()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new OrderDetailRepository(context);
            var orderDetail = new OrderDetail
            {
                OrderID = 1,
                SellerID = 1,
                ProductID = 1,
                Quantity = 2,
                Price = 50.0m
            };
            context.OrderDetails.Add(orderDetail);
            await context.SaveChangesAsync();

            // Act
            orderDetail.Quantity = 3;
            var result = await repository.Update(orderDetail);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Quantity, Is.EqualTo(3));
        }

        [Test]
        public async Task Delete_OrderDetail_ShouldDeleteOrderDetail()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new OrderDetailRepository(context);
            var orderDetail = new OrderDetail
            {
                OrderID = 1,
                ProductID = 1,
                SellerID = 1,
                Quantity = 2,
                Price = 50.0m
            };
            context.OrderDetails.Add(orderDetail);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.Delete(orderDetail.OrderDetailID);

            // Assert
            Assert.NotNull(result);
            Assert.ThrowsAsync<NotFoundException>(async () => await repository.Get(orderDetail.OrderDetailID));
        }

        [Test]
        public async Task Get_OrderDetail_ShouldReturnOrderDetail()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new OrderDetailRepository(context);
            var orderDetail = new OrderDetail
            {
                OrderID = 1,
                ProductID = 1,
                SellerID = 1,
                Quantity = 2,
                Price = 50.0m
            };
            context.OrderDetails.Add(orderDetail);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.Get(orderDetail.OrderDetailID);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.OrderID, Is.EqualTo(orderDetail.OrderID));
        }

        [Test]
        public async Task GetAll_OrderDetails_ShouldReturnOrderDetail()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new OrderDetailRepository(context);
            var orderDetail = new OrderDetail
            {
                OrderID = 1,
                ProductID = 1,
                SellerID = 1,
                Quantity = 2,
                Price = 50.0m
            };
            var orderDetail1 = new OrderDetail
            {
                OrderID = 1,
                ProductID = 2,
                SellerID = 1,
                Quantity = 2,
                Price = 70.0m
            };
            context.OrderDetails.Add(orderDetail);
            context.OrderDetails.Add(orderDetail1);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.Get();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task Get_OrderDetail_ShouldThrowNotFoundException()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new OrderDetailRepository(context);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(async () => await repository.Get(999));
        }
    }
}
