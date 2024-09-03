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
    public class OrderRepositoryTests
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
        public async Task Add_Order_ShouldAddOrder()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new OrderRepository(context);
            var order = new Order
            {
                CustomerID = 1,
                Order_Date = DateTime.Now,
                Status = Enums.OrderStatus.Pending,
                Address = "123 Main St",
                Total_Amount = 100,
                Last_Updated = DateTime.Now,
                Shipping_Method = "dummy"
            };

            // Act
            var result = await repository.Add(order);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.CustomerID, Is.EqualTo(order.CustomerID));
        }

        [Test]
        public async Task Update_Order_ShouldUpdateOrder()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new OrderRepository(context);
            var order = new Order
            {
                CustomerID = 1,
                Order_Date = DateTime.Now,
                Status = Enums.OrderStatus.Pending,
                Address = "123 Main St",
                Total_Amount = 100,
                Last_Updated = DateTime.Now,
                Shipping_Method = "dummy"
            };
            context.Orders.Add(order);
            await context.SaveChangesAsync();

            // Act
            order.Status = Enums.OrderStatus.Delivered;
            var result = await repository.Update(order);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Status, Is.EqualTo(Enums.OrderStatus.Delivered));
        }

        [Test]
        public async Task Delete_Order_ShouldDeleteOrder()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new OrderRepository(context);
            Customer customer = new Customer { Email = "test1@example.com", Name = "John Doe", Account_Status = "Active", Address = "TVL", Phone_Number = "54354453" };
            var order = new Order
            {
                CustomerID = 1,
                Order_Date = DateTime.Now,
                Status = Enums.OrderStatus.Pending,
                Address = "123 Main St",
                Total_Amount = 100,
                Last_Updated = DateTime.Now,
                Shipping_Method = "dummy",
                Customer = customer
            };
            context.Orders.Add(order);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.Delete(order.OrderID);

            // Assert
            Assert.NotNull(result);
            Assert.ThrowsAsync<NotFoundException>(async () => await repository.Get(order.OrderID));
        }

        [Test]
        public async Task Get_Order_ShouldReturnOrder()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new OrderRepository(context);
            Customer customer = new Customer { Email = "test1@example.com", Name = "John Doe", Account_Status = "Active", Address = "TVL", Phone_Number = "54354453" };
            var order = new Order
            {
                CustomerID = 1,
                Order_Date = DateTime.Now,
                Status = Enums.OrderStatus.Pending,
                Address = "123 Main St",
                Total_Amount = 100,
                Last_Updated = DateTime.Now,
                Shipping_Method = "dummy",
                OrderDetails = new List<OrderDetail>(),
                Customer = customer
            };
            context.Orders.Add(order);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.Get(order.OrderID);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.CustomerID, Is.EqualTo(order.CustomerID));
        }

        [Test]
        public async Task Get_Order_ShouldThrowNotFoundException()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new OrderRepository(context);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(async () => await repository.Get(999));
        }

        [Test]
        public async Task Get_AllOrders_ShouldReturnAllOrders()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new OrderRepository(context);
            Customer customer = new Customer { Email = "test1@example.com", Name = "John Doe", Account_Status = "Active", Address = "TVL", Phone_Number = "54354453" };
            context.Orders.AddRange(
                new Order { CustomerID = 1,  Order_Date = DateTime.Now, Status = Enums.OrderStatus.Pending, Address = "123 Main St", Total_Amount = 100, Last_Updated = DateTime.Now, Shipping_Method = "dummy", OrderDetails = new List<OrderDetail>(), Customer = customer },
                new Order { CustomerID = 2,  Order_Date = DateTime.Now, Status = Enums.OrderStatus.Delivered, Address = "456 Elm St", Total_Amount = 150, Last_Updated = DateTime.Now, Shipping_Method = "dummy", OrderDetails = new List<OrderDetail>(), Customer = customer });
            await context.SaveChangesAsync();

            // Act
            var result = await repository.Get();

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count(), Is.EqualTo(2));
        }
    }
}
