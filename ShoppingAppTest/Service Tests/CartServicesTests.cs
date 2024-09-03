using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models.DTO_s.Cart_DTO_s;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Interfaces;
using ShoppingAppAPI.Services.Classes;
using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Repositories.Classes;

namespace ShoppingAppTest.Service_Tests
{
    [TestFixture]
    public class CartServicesTests
    {
        private ICartRepository _mockCartRepository;
        private IRepository<int, CartItem> _mockCartItemRepository;
        private IProductRepository _mockProductRepository;
        private CartServices _cartServices;

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
            var context = GetInMemoryDbContext();
            _mockCartRepository = new CartRepository(context);
            _mockCartItemRepository = new CartItemRepository(context);
            _mockProductRepository = new ProductRepository(context);
           
        }

        [Test]
        public async Task AddItemToCart_CartDoesNotExist_CreatesNewCartAndAddsItem()
        {
            // Arrange
            int cartID = 1;
            int customerID = 123;
            var cartItemDTO = new CartItemGetDTO { ProductID = 1, Quantity = 2};
            IList<Review> reviews = new List<Review>();
            var Seller = new Seller()
            {
                SellerID = 1,
                UserID = 123,
                Email = "seller@example.com",
                Name = "John Doe",
                Address = "123 Main St",
                Phone_Number = "123-456-7890",
                Date_of_Birth = new DateTime(1990, 5, 15),
                Gender = "Male",
                Profile_Picture_URL = "https://example.com/profile.jpg",
                Account_Status = "Active",
                Last_Login = DateTime.Now,
                Products = new List<Product>(), // You can add products here if needed
                OrderDetails = new List<OrderDetail>() // You can add order details here if needed
            };
            var product = new Product { Brand = "Puma", Description = "", Image_URL = "", Name = "Puma Shoe", Seller = Seller, Reviews = reviews};
            await _mockProductRepository.Add(product);

            _cartServices = new CartServices(_mockCartRepository, _mockCartItemRepository, _mockProductRepository);

            // Act
            var result = await _cartServices.AddItemToCart(cartItemDTO, customerID);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.CartItems.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task RemoveItemFromCart_ItemExists_RemovesItem()
        {
            // Arrange
            int cartItemID = 1;
            var cartItem = new CartItem { CartID = 1, ProductID = 1, Quantity = 1, Price = 25.0 };
            var cart = new Cart { CustomerID = 123 };
            IList<Review> reviews = new List<Review>();
            var Seller = new Seller()
            {
                SellerID = 1,
                UserID = 123,
                Email = "seller@example.com",
                Name = "John Doe",
                Address = "123 Main St",
                Phone_Number = "123-456-7890",
                Date_of_Birth = new DateTime(1990, 5, 15),
                Gender = "Male",
                Profile_Picture_URL = "https://example.com/profile.jpg",
                Account_Status = "Active",
                Last_Login = DateTime.Now,
                Products = new List<Product>(), // You can add products here if needed
                OrderDetails = new List<OrderDetail>() // You can add order details here if needed
            };
            var product = new Product { Brand = "Puma", Description = "", Image_URL = "", Name = "Puma Shoe", Seller = Seller, Reviews = reviews };
            await _mockProductRepository.Add(product);
            await _mockCartRepository.Add(cart);
            await _mockCartItemRepository.Add(cartItem);
            _cartServices = new CartServices(_mockCartRepository, _mockCartItemRepository, _mockProductRepository);

            // Act
            var result = await _cartServices.RemoveItemFromCart(cartItemID);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.CartID, Is.EqualTo(cart.CartID));
            });
        }

        [Test]
        public async Task RemoveItemFromCart_ItemExistsMoreThan1_RemovesItem()
        {
            // Arrange
            int cartItemID = 1;
            var cartItem = new CartItem { CartID = 1, ProductID = 1, Quantity = 4, Price = 25.0 };
            var cart = new Cart { CustomerID = 123 };
            IList<Review> reviews = new List<Review>();
            var Seller = new Seller()
            {
                SellerID = 1,
                UserID = 123,
                Email = "seller@example.com",
                Name = "John Doe",
                Address = "123 Main St",
                Phone_Number = "123-456-7890",
                Date_of_Birth = new DateTime(1990, 5, 15),
                Gender = "Male",
                Profile_Picture_URL = "https://example.com/profile.jpg",
                Account_Status = "Active",
                Last_Login = DateTime.Now,
                Products = new List<Product>(), // You can add products here if needed
                OrderDetails = new List<OrderDetail>() // You can add order details here if needed
            };
            var product = new Product { Brand = "Puma", Description = "", Image_URL = "", Name = "Puma Shoe", Seller = Seller, Reviews = reviews };
            await _mockProductRepository.Add(product);
            await _mockCartRepository.Add(cart);
            await _mockCartItemRepository.Add(cartItem);
            _cartServices = new CartServices(_mockCartRepository, _mockCartItemRepository, _mockProductRepository);

            // Act
            var result = await _cartServices.RemoveItemFromCart(cartItemID);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.CartID, Is.EqualTo(cart.CartID));
            });
        }

        [Test]
        public async Task UpdateCartItemQuantity_ItemExists_UpdatesQuantity()
        {
            // Arrange
            int cartItemID = 1;
            int newQuantity = 3;
            var cartItem = new CartItem { CartID = 1, ProductID = 1, Quantity = 1, Price = 25.0 };
            var cart = new Cart { CustomerID = 123 };
            IList<Review> reviews = new List<Review>();
            var Seller = new Seller()
            {
                SellerID = 1,
                UserID = 123,
                Email = "seller@example.com",
                Name = "John Doe",
                Address = "123 Main St",
                Phone_Number = "123-456-7890",
                Date_of_Birth = new DateTime(1990, 5, 15),
                Gender = "Male",
                Profile_Picture_URL = "https://example.com/profile.jpg",
                Account_Status = "Active",
                Last_Login = DateTime.Now,
                Products = new List<Product>(), // You can add products here if needed
                OrderDetails = new List<OrderDetail>() // You can add order details here if needed
            };
            var product = new Product { Brand = "Puma", Description = "", Image_URL = "", Name = "Puma Shoe", Seller = Seller, Reviews = reviews };
            await _mockProductRepository.Add(product);
            await _mockCartRepository.Add(cart);
            await _mockCartItemRepository.Add(cartItem);
            _cartServices = new CartServices(_mockCartRepository, _mockCartItemRepository, _mockProductRepository);


            // Act
            var result = await _cartServices.UpdateCartItemQuantity(cartItemID, newQuantity);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.CartID, Is.EqualTo(cart.CartID));
            Assert.Multiple(() =>
            {
                Assert.That(result.CartItems.First().Quantity, Is.EqualTo(newQuantity));
                Assert.That(result.CartItems.First().Price, Is.EqualTo(product.Price * newQuantity));
            });
        }

        [Test]
        public async Task GetCart_CartExists_ReturnsCart()
        {
            // Arrange
            var cartItem = new CartItem { CartID = 1, ProductID = 1, Quantity = 1, Price = 25.0 };
            var cart = new Cart { CustomerID = 123 };
            int cartID = 1;
            IList<Review> reviews = new List<Review>();
            var Seller = new Seller()
            {
                SellerID = 1,
                UserID = 123,
                Email = "seller@example.com",
                Name = "John Doe",
                Address = "123 Main St",
                Phone_Number = "123-456-7890",
                Date_of_Birth = new DateTime(1990, 5, 15),
                Gender = "Male",
                Profile_Picture_URL = "https://example.com/profile.jpg",
                Account_Status = "Active",
                Last_Login = DateTime.Now,
                Products = new List<Product>(), // You can add products here if needed
                OrderDetails = new List<OrderDetail>() // You can add order details here if needed
            };
            var product = new Product { Brand = "Puma", Description = "", Image_URL = "", Name = "Puma Shoe", Seller = Seller, Reviews = reviews };
            await _mockProductRepository.Add(product);
            await _mockCartRepository.Add(cart);
            await _mockCartItemRepository.Add(cartItem);
            _cartServices = new CartServices(_mockCartRepository, _mockCartItemRepository, _mockProductRepository);



            // Act
            var result = await _cartServices.GetCart(cartID);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.CartID, Is.EqualTo(cartID));
        }

        [Test]
        public async Task CloseCart_CartExists_ClosesCart()
        {
            // Arrange
            int cartID = 1;
            var cart = new Cart { CartID = cartID, CustomerID = 123, CartItems = new List<CartItem>() };
            await _mockCartRepository.Add(cart);
            _cartServices = new CartServices(_mockCartRepository, _mockCartItemRepository, _mockProductRepository);

            // Act
            var result = await _cartServices.CloseCart(cartID);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.CartID, Is.EqualTo(cartID));
        }

        [Test]
        public void CloseCart_CartDoesNotExist_ThrowsException()
        {
            // Arrange
            int cartID = 1;

            _cartServices = new CartServices(_mockCartRepository, _mockCartItemRepository, _mockProductRepository);

            // Act & Assert
            var ex = Assert.ThrowsAsync<UnableToUpdateItemException>(async () => await _cartServices.CloseCart(cartID));
            Assert.That(ex.Message, Is.EqualTo("Cart with given ID Not Found!"));
        }

        [Test]
        public async Task AddItemToCart_ExistingCart_AddsOrUpdatesItem()
        {
            // Arrange
            int cartID = 1;
            int customerID = 123;
            var cartItemDTO = new CartItemGetDTO { ProductID = 1, Quantity = 2 };
            var cart = new Cart
            {
                CartID = cartID,
                CustomerID = customerID,
                CartItems = new List<CartItem>
                {
                    new CartItem { CartItemID = 1, CartID = cartID, ProductID = cartItemDTO.ProductID, Quantity = 1, Price = 25.0 }
                }
            };
            IList<Review> reviews = new List<Review>();
            var Seller = new Seller()
            {
                SellerID = 1,
                UserID = 123,
                Email = "seller@example.com",
                Name = "John Doe",
                Address = "123 Main St",
                Phone_Number = "123-456-7890",
                Date_of_Birth = new DateTime(1990, 5, 15),
                Gender = "Male",
                Profile_Picture_URL = "https://example.com/profile.jpg",
                Account_Status = "Active",
                Last_Login = DateTime.Now,
                Products = new List<Product>(), // You can add products here if needed
                OrderDetails = new List<OrderDetail>() // You can add order details here if needed
            };
            var product = new Product { Brand = "Puma", Description = "", Image_URL = "", Name = "Puma Shoe", Seller = Seller, Reviews = reviews, Price=25 };
            await _mockProductRepository.Add(product);
            await _mockCartRepository.Add(cart);
            _cartServices = new CartServices(_mockCartRepository, _mockCartItemRepository, _mockProductRepository);


            // Act
            var result = await _cartServices.AddItemToCart(cartItemDTO, customerID);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.CartItems.Count(), Is.EqualTo(1));
            var updatedItem = result.CartItems.First();
            Assert.That(updatedItem.Quantity, Is.EqualTo(3));
            Assert.That(updatedItem.Price, Is.EqualTo(75.0));
        }

        [Test]
        public async Task AddItemToCart_CartExist_CreatingNewItem()
        {
            // Arrange
            int cartID = 1;
            var cartItemDTO = new CartItemGetDTO { ProductID = 1, Quantity = 2 };
            IList<Review> reviews = new List<Review>();
            var Seller = new Seller()
            {
                SellerID = 1,
                UserID = 123,
                Email = "seller@example.com",
                Name = "John Doe",
                Address = "123 Main St",
                Phone_Number = "123-456-7890",
                Date_of_Birth = new DateTime(1990, 5, 15),
                Gender = "Male",
                Profile_Picture_URL = "https://example.com/profile.jpg",
                Account_Status = "Active",
                Last_Login = DateTime.Now,
                Products = new List<Product>(), // You can add products here if needed
                OrderDetails = new List<OrderDetail>() // You can add order details here if needed
            };
            var product = new Product { Brand = "Puma", Description = "", Image_URL = "", Name = "Puma Shoe", Seller = Seller, Reviews = reviews, Price = 25 };
            var product1 = new Product { Brand = "Puma", Description = "", Image_URL = "", Name = "Puma Shoe", Seller = Seller, Reviews = reviews, Price = 25 };
            var cartItem = new CartItem { CartItemID = 1, CartID = cartID, ProductID = 2, Quantity = 1, Price = 25.0 };
            var cart = new Cart
            {
                CartID = cartID,
                CustomerID = 1,
            };
            await _mockProductRepository.Add(product);
            await _mockProductRepository.Add(product1);
            await _mockCartRepository.Add(cart);
            await _mockCartItemRepository.Add(cartItem);
            _cartServices = new CartServices(_mockCartRepository, _mockCartItemRepository, _mockProductRepository);


            // Act
            var result = await _cartServices.AddItemToCart(cartItemDTO, 1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.CartItems.Count(), Is.EqualTo(2));
        }
    }
}
