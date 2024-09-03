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
    public class ProductRepositoryTests
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
        public async Task Add_Product_ShouldAddProduct()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new ProductRepository(context);
            var product = new Product
            {
                SellerID = 1,
                Name = "Product 1",
                Description = "Description 1",
                Price = 100.0m,
                CategoryID = 1,
                Brand = "Brand 1",
                Image_URL = "http://example.com/image1.jpg",
                Stock_Quantity = 10,
                Creation_Date = DateTime.Now,
                Last_Updated = DateTime.Now
            };

            // Act
            var result = await repository.Add(product);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Name, Is.EqualTo(product.Name));
        }

        [Test]
        public async Task Update_Product_ShouldUpdateProduct()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new ProductRepository(context);
            var product = new Product
            {
                SellerID = 1,
                Name = "Product 1",
                Description = "Description 1",
                Price = 100.0m,
                CategoryID = 1,
                Brand = "Brand 1",
                Image_URL = "http://example.com/image1.jpg",
                Stock_Quantity = 10,
                Creation_Date = DateTime.Now,
                Last_Updated = DateTime.Now
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            // Act
            product.Name = "Updated Product 1";
            var result = await repository.Update(product);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Name, Is.EqualTo("Updated Product 1"));
        }

        [Test]
        public async Task Delete_Product_ShouldDeleteProduct()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new ProductRepository(context);
            Seller seller = new Seller
            {
                Name = "ABC",
                UserID = 1,
                Email = "abc@gmail.com",
                Address = "US",
                Phone_Number = "43432532",
                Account_Status = "Active",
            };
            var product = new Product
            {
                SellerID = 1,
                Name = "Product 1",
                Description = "Description 1",
                Price = 100.0m,
                CategoryID = 1,
                Brand = "Brand 1",
                Image_URL = "http://example.com/image1.jpg",
                Stock_Quantity = 10,
                Creation_Date = DateTime.Now,
                Last_Updated = DateTime.Now,
                Seller= seller,
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.Delete(product.ProductID);

            // Assert
            Assert.NotNull(result);
            Assert.ThrowsAsync<NotFoundException>(async () => await repository.Get(product.ProductID));
        }

        [Test]
        public async Task Get_Product_ShouldReturnProduct()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new ProductRepository(context);
            Seller seller = new Seller
            {
                Name = "ABC",
                UserID = 1,
                Email = "abc@gmail.com",
                Address = "US",
                Phone_Number = "43432532",
                Account_Status = "Active",
            };
            var product = new Product
            {
                SellerID = 1,
                Name = "Product 1",
                Description = "Description 1",
                Price = 100.0m,
                CategoryID = 1,
                Brand = "Brand 1",
                Image_URL = "http://example.com/image1.jpg",
                Stock_Quantity = 10,
                Creation_Date = DateTime.Now,
                Last_Updated = DateTime.Now,
                Seller = seller
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.Get(product.ProductID);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(product.Name));
        }



        [Test]
        public async Task Get_Product_ShouldThrowNotFoundException()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new ProductRepository(context);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(async () => await repository.Get(999));
        }

        [Test]
        public async Task Get_AllProducts_ShouldReturnAllProducts()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new ProductRepository(context);
            Seller seller = new Seller
            {
                Name = "ABC",
                UserID = 1,
                Email = "abc@gmail.com",
                Address = "US",
                Phone_Number = "43432532",
                Account_Status = "Active",
            };
            context.Products.AddRange(
                new Product { SellerID = 1, Name = "Product 1", Description = "Description 1", Price = 100.0m, CategoryID = 1, Brand = "Brand 1", Image_URL = "http://example.com/image1.jpg", Stock_Quantity = 10, Creation_Date = DateTime.Now, Last_Updated = DateTime.Now, Seller = seller },
                new Product { SellerID = 2, Name = "Product 2", Description = "Description 2", Price = 200.0m, CategoryID = 2, Brand = "Brand 2", Image_URL = "http://example.com/image2.jpg", Stock_Quantity = 20, Creation_Date = DateTime.Now, Last_Updated = DateTime.Now, Seller = seller });
            await context.SaveChangesAsync();

            // Act
            var result = await repository.Get();

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetProductByName_ShouldReturnProduct()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new ProductRepository(context);
            Seller seller = new Seller
            {
                Name = "ABC",
                UserID = 1,
                Email = "abc@gmail.com",
                Address = "US",
                Phone_Number = "43432532",
                Account_Status = "Active",
            };
            var product = new Product
            {
                SellerID = 1,
                Name = "Product 1",
                Description = "Description 1",
                Price = 100.0m,
                CategoryID = 1,
                Brand = "Brand 1",
                Image_URL = "http://example.com/image1.jpg",
                Stock_Quantity = 10,
                Creation_Date = DateTime.Now,
                Last_Updated = DateTime.Now,
                Seller = seller
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetProductByName(product.Name);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(product.Name));
        }

        [Test]
        public async Task GetProductByName_ShouldReturnNull_WhenProductNotFound()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new ProductRepository(context);

            // Act
            var result = await repository.GetProductByName("NonExistingProduct");

            // Assert
            Assert.That(result, Is.Null);
        }
    }
}
