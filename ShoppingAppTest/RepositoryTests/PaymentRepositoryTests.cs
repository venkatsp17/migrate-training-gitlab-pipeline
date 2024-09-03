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
    public class PaymentRepositoryTests
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
        public async Task Add_Payment_ShouldAddPayment()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new PaymentRepository(context);
            var payment = new Payment
            {
                OrderID = 1,
                Payment_Method = "Credit Card",
                Amount = 100.0m,
                Transaction_Date = DateTime.Now,
                Payment_Status = Enums.PaymentStatus.Authorized
            };

            // Act
            var result = await repository.Add(payment);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.OrderID, Is.EqualTo(payment.OrderID));
            Assert.That(result.Payment_Method, Is.EqualTo(payment.Payment_Method));
        }

        [Test]
        public async Task Update_Payment_ShouldUpdatePayment()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new PaymentRepository(context);
            var payment = new Payment
            {
                OrderID = 1,
                Payment_Method = "Credit Card",
                Amount = 100.0m,
                Transaction_Date = DateTime.Now,
                Payment_Status = Enums.PaymentStatus.Pending
            };
            context.Payments.Add(payment);
            await context.SaveChangesAsync();

            // Act
            payment.Payment_Status = Enums.PaymentStatus.Authorized;
            var result = await repository.Update(payment);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Payment_Status, Is.EqualTo(Enums.PaymentStatus.Authorized));
        }

        [Test]
        public async Task Delete_Payment_ShouldDeletePayment()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new PaymentRepository(context);
            var payment = new Payment
            {
                OrderID = 1,
                Payment_Method = "Credit Card",
                Amount = 100.0m,
                Transaction_Date = DateTime.Now,
                Payment_Status = Enums.PaymentStatus.Pending
            };
            context.Payments.Add(payment);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.Delete(payment.PaymentID);

            // Assert
            Assert.NotNull(result);
            Assert.ThrowsAsync<NotFoundException>(async () => await repository.Get(payment.PaymentID));
        }

        [Test]
        public async Task Get_Payment_ShouldReturnPayment()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new PaymentRepository(context);
            var payment = new Payment
            {
                OrderID = 1,
                Payment_Method = "Credit Card",
                Amount = 100.0m,
                Transaction_Date = DateTime.Now,
                Payment_Status = Enums.PaymentStatus.Authorized
            };
            context.Payments.Add(payment);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.Get(payment.PaymentID);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.OrderID, Is.EqualTo(payment.OrderID));
        }

        [Test]
        public async Task GetAll_Payments_ShouldReturnPayment()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new PaymentRepository(context);
            var payment = new Payment
            {
                OrderID = 1,
                Payment_Method = "Credit Card",
                Amount = 100.0m,
                Transaction_Date = DateTime.Now,
                Payment_Status = Enums.PaymentStatus.Authorized
            };
            var payment1 = new Payment
            {
                OrderID = 2,
                Payment_Method = "Credit Card",
                Amount = 100.0m,
                Transaction_Date = DateTime.Now,
                Payment_Status = Enums.PaymentStatus.Authorized
            };

            context.Payments.Add(payment);
            context.Payments.Add(payment1);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.Get();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task Get_Payment_ShouldThrowNotFoundException()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new PaymentRepository(context);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(async () => await repository.Get(999));
        }
    }
}
