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
    public class RefundRepositoryTests
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
        public async Task Add_Refund_ShouldAddRefund()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new RefundRepository(context);
            var refund = new Refund
            {
                OrderID = 1,
                Amount = 50.0m,
                Refund_Date = DateTime.Now,
                Reason = "Product damaged",
                Refund_Method = "Credit Card",
                Status = Enums.RefundStatus.Pending
            };

            // Act
            var result = await repository.Add(refund);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Amount, Is.EqualTo(refund.Amount));
        }

        [Test]
        public async Task Update_Refund_ShouldUpdateRefund()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new RefundRepository(context);
            var refund = new Refund
            {
                OrderID = 1,
                Amount = 50.0m,
                Refund_Date = DateTime.Now,
                Reason = "Product damaged",
                Refund_Method = "Credit Card",
                Status = Enums.RefundStatus.Pending
            };
            context.Refunds.Add(refund);
            await context.SaveChangesAsync();

            // Act
            refund.Status = Enums.RefundStatus.Approved;
            var result = await repository.Update(refund);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Status, Is.EqualTo(Enums.RefundStatus.Approved));
        }

        [Test]
        public async Task Delete_Refund_ShouldDeleteRefund()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new RefundRepository(context);
            var refund = new Refund
            {
                OrderID = 1,
                Amount = 50.0m,
                Refund_Date = DateTime.Now,
                Reason = "Product damaged",
                Refund_Method = "Credit Card",
                Status = Enums.RefundStatus.Pending
            };
            context.Refunds.Add(refund);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.Delete(refund.RefundID);

            // Assert
            Assert.NotNull(result);
            Assert.ThrowsAsync<NotFoundException>(async () => await repository.Get(refund.RefundID));
        }

        [Test]
        public async Task Get_Refund_ShouldReturnRefund()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new RefundRepository(context);
            var refund = new Refund
            {
                OrderID = 1,
                Amount = 50.0m,
                Refund_Date = DateTime.Now,
                Reason = "Product damaged",
                Refund_Method = "Credit Card",
                Status = Enums.RefundStatus.Pending
            };
            context.Refunds.Add(refund);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.Get(refund.RefundID);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Amount, Is.EqualTo(refund.Amount));
        }

        [Test]
        public async Task GetAll_Refund_ShouldReturnRefund()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new RefundRepository(context);
            var refund = new Refund
            {
                OrderID = 1,
                Amount = 50.0m,
                Refund_Date = DateTime.Now,
                Reason = "Product damaged",
                Refund_Method = "Credit Card",
                Status = Enums.RefundStatus.Pending
            };
            var refund1 = new Refund
            {
                OrderID = 2,
                Amount = 50.0m,
                Refund_Date = DateTime.Now,
                Reason = "Product damaged",
                Refund_Method = "Credit Card",
                Status = Enums.RefundStatus.Pending
            };
            context.Refunds.Add(refund);
            context.Refunds.Add(refund1);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.Get();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task Get_Refund_ShouldThrowNotFoundException()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new RefundRepository(context);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(async () => await repository.Get(999));
        }
    }
}
