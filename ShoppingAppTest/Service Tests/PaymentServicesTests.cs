using Microsoft.EntityFrameworkCore;
using Moq;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models.DTO_s.Payment_DTO_s;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Classes;
using ShoppingAppAPI.Repositories.Interfaces;
using ShoppingAppAPI.Services.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingAppTest.Service_Tests
{
    [TestFixture]
    public class PaymentServicesTests
    {
        private ShoppingAppContext _context;
        private IRepository<int, Payment> _paymentRepository;
        private IOrderRepository _orderRepository;
        private PaymentServices _paymentServices;

        private ShoppingAppContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ShoppingAppContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new ShoppingAppContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        [SetUp]
        public void Setup()
        {
            _context = GetInMemoryDbContext();
            _paymentRepository = new PaymentRepository(_context);
            _orderRepository = new OrderRepository(_context);

            var customer = new Customer
            {
                CustomerID = 1,
                Name = "John Doe",
                Email = "john.doe@example.com",
                Address = "123 Main St",
                Phone_Number = "123-456-7890",
                Profile_Picture_URL = "http://example.com/profile.jpg",
                Last_Login = DateTime.Now,
                Account_Status = "Active"

            };
            // Seed the database with test data
            var order = new Order
            {
                OrderID = 1,
                CustomerID = 1,
                Order_Date = DateTime.Now,
                Status = Enums.OrderStatus.Pending,
                Address = "123 Main St",
                Total_Amount = 200.0m,
                Shipping_Method = "Standard",
                Shipping_Cost = 5.0m,
                Customer = customer,
            };
            _orderRepository.Add(order);
            _paymentServices = new PaymentServices(_paymentRepository, _orderRepository);
        }

        [Test]
        public async Task MakePayment_ValidPayment_ReturnsPaymentReturnDTO()
        {
            // Arrange
            var paymentGetDTO = new PaymentGetDTO
            {
                OrderID = 1,
                Payment_Method = "Credit Card",
            };

            // Act
            var result = await _paymentServices.MakePayment(paymentGetDTO);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.OrderID, Is.EqualTo(paymentGetDTO.OrderID));
        }

        [Test]
        public void MakePayment_InvalidOrder_ThrowsUnableToProcessOrderException()
        {
            // Arrange
            var paymentGetDTO = new PaymentGetDTO
            {
                OrderID = 999, // Non-existing OrderID
                Payment_Method = "Credit Card",
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<UnableToAddItemException>(async () => await _paymentServices.MakePayment(paymentGetDTO));
            Assert.That(ex.Message, Is.EqualTo("Unable to add payment at this moment: Order with given ID Not Found!"));
        }

        [Test]
        public async Task MakePayment_CanceledOrder_ThrowsUnableToProcessOrderException()
        {
            // Arrange
            var order = await _context.Orders.FindAsync(1);
            order.Status = Enums.OrderStatus.Canceled;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            var paymentGetDTO = new PaymentGetDTO
            {
                OrderID = 1,
                Payment_Method = "Credit Card",
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<UnableToAddItemException>(async () => await _paymentServices.MakePayment(paymentGetDTO));
            Assert.That(ex.Message, Is.EqualTo("Unable to add payment at this moment: Unable to Process Order at this moment!"));
        }

        [Test]
        public async Task MakePayment_AddPaymentFails_ThrowsUnableToAddItemException()
        {
            // Arrange
            var paymentGetDTO = new PaymentGetDTO
            {
                OrderID = 1,
                Payment_Method = "Credit Card",
            };

            // Simulate failure to add payment by using a mock repository that returns null on add
            var mockPaymentRepository = new Mock<IRepository<int, Payment>>();
            mockPaymentRepository.Setup(repo => repo.Add(It.IsAny<Payment>())).ReturnsAsync((Payment)null);

            var paymentServicesWithMockRepo = new PaymentServices(mockPaymentRepository.Object, _orderRepository);

            // Act & Assert
            var ex = Assert.ThrowsAsync<UnableToAddItemException>(async () => await paymentServicesWithMockRepo.MakePayment(paymentGetDTO));
            Assert.That(ex.Message, Is.EqualTo("Unable to add payment at this moment: Unable to add payment at this moment!"));
        }
    }
}
