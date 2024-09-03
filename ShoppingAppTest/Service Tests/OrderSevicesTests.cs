using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models.DTO_s.Order_DTO_s;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Classes;
using ShoppingAppAPI.Repositories.Interfaces;
using ShoppingAppAPI.Services.Classes;
using ShoppingAppAPI.Services.Interfaces;
using static ShoppingAppAPI.Models.Enums;
using Microsoft.Data.Sqlite;
using System.Text;
using Castle.Core.Resource;

namespace ShoppingAppTest.Service_Tests
{
    [TestFixture]
    public class OrderServicesTests
    {
        private ShoppingAppContext _context;
        private IOrderRepository _orderRepository;
        private IRepository<int, Refund> _refundRepository;
        private IRepository<int, CartItem> _cartItemRepository;
        private IOrderDetailRepository _orderDetailRepository;
        private ICartRepository _cartRepository;
        private ICustomerRepository _customerRepository;
        private IProductRepository _productRepository;
        private ICartServices _cartServices;
        private IUnitOfWork _unitOfWork;
        private OrderServices _orderServices;

        private ShoppingAppContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ShoppingAppContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new ShoppingAppContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        private ShoppingAppContext GetInMemorySqlDbContext()
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

        [SetUp]
        public void Setup()
        {
            _context = GetInMemoryDbContext();
            _orderRepository = new OrderRepository(_context);
            _refundRepository = new RefundRepository(_context);
            _orderDetailRepository = new OrderDetailRepository(_context);
            _cartRepository = new CartRepository(_context);
            _customerRepository = new CustomerRepository(_context);
            _productRepository = new ProductRepository(_context);
            _cartItemRepository = new CartItemRepository(_context);
            _unitOfWork = new UnitOfWorkServices(_context);

            // Seed the database with test data
            var product = new Product
            {
                ProductID = 1,
                Name = "Test Product",
                Price = 100.0m,
                SellerID = 1,
                Description = "",
                CategoryID = 1,
                Brand ="",
                Image_URL = "",
                Stock_Quantity =10,
                Last_Updated=DateTime.Now

            };
            var cartItem = new CartItem
            {
                CartID = 1,
                ProductID = product.ProductID,
                Quantity = 2,
                Price = (double)product.Price
            };
            var cart = new Cart
            {
                CartID = 1,
                CustomerID = 1,
                Cart_Status = CartStatus.Open,
                CartItems = new List<CartItem> { cartItem }
            };
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

            _productRepository.Add(product);
            _cartItemRepository.Add(cartItem);
            _cartRepository.Add(cart);
            _customerRepository.Add(customer);

           
            _cartServices = new CartServices(_cartRepository, _cartItemRepository, _productRepository);
            _orderServices = new OrderServices(_orderRepository, _unitOfWork, _orderDetailRepository, _cartRepository, _cartServices, _refundRepository);
        }

         private async Task SeedSqlDatabase()
        {
            _context = GetInMemorySqlDbContext();
            _orderRepository = new OrderRepository(_context);
            _refundRepository = new RefundRepository(_context);
            _orderDetailRepository = new OrderDetailRepository(_context);
            _cartRepository = new CartRepository(_context);
            _customerRepository = new CustomerRepository(_context);
            _productRepository = new ProductRepository(_context);
            _cartItemRepository = new CartItemRepository(_context);
            _unitOfWork = new UnitOfWorkServices(_context);
            var newUser = new User { UserID = 1, Username = "User3", Password = Encoding.UTF8.GetBytes("password"), Password_Hashkey = Encoding.UTF8.GetBytes("passwordHash") };
            var newUser1 = new User { UserID = 2, Username = "User3", Password = Encoding.UTF8.GetBytes("password"), Password_Hashkey = Encoding.UTF8.GetBytes("passwordHash") };
            var customer = new Customer
            {
                CustomerID = 1,
                Name = "John Doe",
                Email = "john.doe@example.com",
                Address = "123 Main St",
                Phone_Number = "123-456-7890",
                Profile_Picture_URL = "http://example.com/profile.jpg",
                Last_Login = DateTime.Now,
                Account_Status = "Active",
                UserID = 2
            };
            var seller = new Seller
            {
                SellerID = 1,
                Name = "John Doe",
                Email = "john.doe@example.com",
                Address = "123 Main St",
                Phone_Number = "123-456-7890",
                Profile_Picture_URL = "http://example.com/profile.jpg",
                Last_Login = DateTime.Now,
                Account_Status = "Active",
                UserID = 1
            };
            var category = new Category
            {
                CategoryID = 1,
                Name = "Electronics",

            };
            var product = new Product
            {
                ProductID = 1,
                Name = "Test Product",
                Price = 100.0m,
                SellerID = 1,
                Description = "",
                CategoryID = 1,
                Brand = "",
                Image_URL = "",
                Stock_Quantity = 10,
                Last_Updated = DateTime.Now

            };
          
            _context.Users.Add(newUser);
            _context.Users.Add(newUser1);
            await _context.SaveChangesAsync();
            _context.Sellers.Add(seller);
            await _context.SaveChangesAsync();
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            await _productRepository.Add(product);
            await _customerRepository.Add(customer);
        }

        [Test]
        public async Task PlaceOrder_ValidOrder_ReturnsCustomerOrderReturnDTO()
        {
            await SeedSqlDatabase();
            var cart = new Cart
            {
                CartID = 1,
                CustomerID = 1,
                Cart_Status = CartStatus.Open,
            };
            var cartItem = new CartItem
            {
                CartID = 1,
                ProductID = 1,
                Quantity = 2,
                Price = (double)100.0m,
            };

            await _cartRepository.Add(cart);
            await _cartItemRepository.Add(cartItem);
            _cartServices = new CartServices(_cartRepository, _cartItemRepository, _productRepository);
            _orderServices = new OrderServices(_orderRepository, _unitOfWork, _orderDetailRepository, _cartRepository, _cartServices, _refundRepository);
            // Arrange
            var placeOrderDTO = new PlaceOrderDTO
            {
                CustomerID = 1,
                Address = "123 Main St",
                Shipping_Method = "Standard",
                Shipping_Cost = 5.0m
            };

            // Act
            var result = await _orderServices.PlaceOrder(placeOrderDTO);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.OrderDetails, Is.Not.Empty);
        }

        [Test]
        public async Task PlaceOrder_EmptyCart_ThrowsEmptyCartException()
        {
            await SeedSqlDatabase();
            // Arrange
            var emptyCart = new Cart
            {
                CartID = 1,
                CustomerID = 1,
                Cart_Status = CartStatus.Empty
            };
            await _cartRepository.Add(emptyCart);
            _cartServices = new CartServices(_cartRepository, _cartItemRepository, _productRepository);
            _orderServices = new OrderServices(_orderRepository, _unitOfWork, _orderDetailRepository, _cartRepository, _cartServices, _refundRepository);

            var placeOrderDTO = new PlaceOrderDTO
            {
                CustomerID = 1,
                Address = "123 Main St",
                Shipping_Method = "Standard",
                Shipping_Cost = 5.0m
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<UnableToAddItemException>(async () => await _orderServices.PlaceOrder(placeOrderDTO));
            Assert.That(ex.Message, Is.EqualTo("Cart Is Empty!"));
        }

        [Test]
        public async Task UpdateOrderStatus_ValidOrder_ReturnsUpdatedOrder()
        {
            // Arrange
            var order = new Order
            {
                CustomerID = 1,
                Order_Date = DateTime.Now,
                Status = OrderStatus.Pending,
                Address = "123 Main St",
                Total_Amount = 200.0m,
                Shipping_Method = "Standard",
                Shipping_Cost = 5.0m
            };
            await _orderRepository.Add(order);

            // Act
            var result = await _orderServices.UpdateOrderStatus(OrderStatus.Shipped, order.OrderID);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.EqualTo(OrderStatus.Shipped));
        }

        [Test]
        public async Task ViewAllSellerActiveOrders_ValidSeller_ReturnsActiveOrders()
        {
            // Arrange
            var customer = new Customer
            {
                CustomerID = 2,
                Name = "John Doe",
                Email = "john.doe@example.com",
                Address = "123 Main St",
                Phone_Number = "123-456-7890",
                Profile_Picture_URL = "http://example.com/profile.jpg",
                Last_Login = DateTime.Now,
                Account_Status = "Active"

            };
            var order = new Order
            {
                CustomerID = 2,
                Order_Date = DateTime.Now,
                Status = OrderStatus.Pending,
                Address = "123 Main St",
                Total_Amount = 200.0m,
                Shipping_Method = "Standard",
                Shipping_Cost = 5.0m,
                Customer = customer
            };
            var orderDetail = new OrderDetail
            {
                ProductID = 1,
                Quantity = 2,
                OrderID = order.OrderID,
                SellerID = 1,
                Price = 100.0m,
                Order = order,
            };
            await _orderRepository.Add(order);
            await _orderDetailRepository.Add(orderDetail);
            _orderServices = new OrderServices(_orderRepository, _unitOfWork, _orderDetailRepository, _cartRepository, _cartServices, _refundRepository);
            // Act
            var result = await _orderServices.ViewAllSellerActiveOrders(1);

            // Assert
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        public async Task ViewCustomerOrderHistory_OrdersExist_ReturnsOrderHistory()
        {
            // Arrange
            int customerId = 1;
            var orders = new List<Order>
            {
                new Order { OrderID = 1, CustomerID = customerId, Status = OrderStatus.Delivered, Address = "", Shipping_Method = "" },
                new Order { OrderID = 2, CustomerID = customerId, Status = OrderStatus.Canceled, Address = "", Shipping_Method = ""  },
                new Order { OrderID = 3, CustomerID = customerId, Status = OrderStatus.Refunded, Address = "", Shipping_Method = ""  }
            };
            _context.Orders.AddRange(orders);
            await _context.SaveChangesAsync();

            // Act
            var result = await _orderServices.ViewCustomerOrderHistory(customerId);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public void ViewCustomerOrderHistory_NoOrders_ThrowsNoAvailableItemException()
        {
            // Act & Assert
            Assert.ThrowsAsync<NoAvailableItemException>(async () => await _orderServices.ViewCustomerOrderHistory(1));
        }

        [Test]
        public async Task CustomerCancelOrder_OrderExists_OrderCanceled()
        {
            // Arrange
            await SeedSqlDatabase();
            int customerId = 1;
            int orderId = 1;
            var order = new Order { OrderID = 1, CustomerID = customerId, Status = OrderStatus.Processing, Address = "", Shipping_Method = "" };
            await _orderRepository.Add(order);
            _cartServices = new CartServices(_cartRepository, _cartItemRepository, _productRepository);
            _orderServices = new OrderServices(_orderRepository, _unitOfWork, _orderDetailRepository, _cartRepository, _cartServices, _refundRepository);
            // Act
            var result = await _orderServices.CustomerCancelOrder(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(OrderStatus.Canceled, result.Status);
        }

        [Test]
        public async Task CustomerCancelOrderWithSuccessPayment_OrderExists_OrderCanceled()
        {
            // Arrange
            await SeedSqlDatabase();
            int customerId = 1;
            int orderId = 1;
            var order = new Order { OrderID = 1, Success_PaymentID =1, CustomerID = customerId, Status = OrderStatus.Processing, Address = "", Shipping_Method = "" };
            await _orderRepository.Add(order);
            _cartServices = new CartServices(_cartRepository, _cartItemRepository, _productRepository);
            _orderServices = new OrderServices(_orderRepository, _unitOfWork, _orderDetailRepository, _cartRepository, _cartServices, _refundRepository);
            // Act
            var result = await _orderServices.CustomerCancelOrder(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(OrderStatus.Canceled, result.Status);
        }

        [Test]
        public async Task CustomerCancelOrder_OrderNotFound_ThrowsNotFoundException()
        {
            await SeedSqlDatabase();
            var context = GetInMemorySqlDbContext();
            int customerId = 1;
            _cartServices = new CartServices(_cartRepository, _cartItemRepository, _productRepository);
            _orderServices = new OrderServices(_orderRepository, _unitOfWork, _orderDetailRepository, _cartRepository, _cartServices, _refundRepository);
            // Act & Assert
            Assert.ThrowsAsync<UnableToUpdateItemException>(async () => await _orderServices.CustomerCancelOrder(1));
        }

        [Test]
        public async Task CustomerCancelOrder_OrderAlreadyCompleted_ThrowsNotAllowedToCancelOrderException()
        {
            // Arrange
            await SeedSqlDatabase();
            int orderId = 1;
            var order = new Order { OrderID = orderId, CustomerID =1, Status = OrderStatus.Delivered, Address="", Shipping_Method = "" };
            await _orderRepository.Add(order);
            _cartServices = new CartServices(_cartRepository, _cartItemRepository, _productRepository);
            _orderServices = new OrderServices(_orderRepository, _unitOfWork, _orderDetailRepository, _cartRepository, _cartServices, _refundRepository);

            // Act & Assert
            Assert.ThrowsAsync<UnableToUpdateItemException>(async () => await _orderServices.CustomerCancelOrder(orderId));
        }

        [Test]
        public async Task UpdateOrderDeliveryDetails_OrderExists_UpdatesOrder()
        {
            // Arrange
            await SeedSqlDatabase();
            var updateOrderDeliveryDetailsDTO = new UpdateOrderDeliveryDetailsDTO
            {
                OrderID = 1,
                Address = "New Address",
                Shipping_Method = "Express"
            };
            var order = new Order { OrderID = updateOrderDeliveryDetailsDTO.OrderID, CustomerID = 1, Address = "Old Address", Shipping_Method = "Standard" };
            await _orderRepository.Add(order);
            _cartServices = new CartServices(_cartRepository, _cartItemRepository, _productRepository);
            _orderServices = new OrderServices(_orderRepository, _unitOfWork, _orderDetailRepository, _cartRepository, _cartServices, _refundRepository);

            // Act
            var result = await _orderServices.UpdateOrderDeliveryDetails(updateOrderDeliveryDetailsDTO);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(updateOrderDeliveryDetailsDTO.Address, result.Address);
            Assert.AreEqual(updateOrderDeliveryDetailsDTO.Shipping_Method, result.Shipping_Method);
        }

        [Test]
        public async Task UpdateOrderDeliveryDetails_OrderNotFound_ThrowsNotFoundException()
        {
            await SeedSqlDatabase();
            // Arrange
            var updateOrderDeliveryDetailsDTO = new UpdateOrderDeliveryDetailsDTO { OrderID = 1, Address = "", Shipping_Method="" };
            _cartServices = new CartServices(_cartRepository, _cartItemRepository, _productRepository);
            _orderServices = new OrderServices(_orderRepository, _unitOfWork, _orderDetailRepository, _cartRepository, _cartServices, _refundRepository);
            // Act & Assert
            Assert.ThrowsAsync<UnableToUpdateItemException>(async () => await _orderServices.UpdateOrderDeliveryDetails(updateOrderDeliveryDetailsDTO));
        }
    }
}
