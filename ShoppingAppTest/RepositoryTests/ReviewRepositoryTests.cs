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
    public class ReviewRepositoryTests
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
        public async Task Add_Review_ShouldAddReview()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new ReviewRepository(context);
            var review = new Review
            {
                ProductID = 1,
                CustomerID = 1,
                Rating = 5,
                Comment = "Excellent product!",
                Review_Date = DateTime.Now
            };

            // Act
            var result = await repository.Add(review);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Rating, Is.EqualTo(review.Rating));
        }

        [Test]
        public async Task Update_Review_ShouldUpdateReview()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new ReviewRepository(context);
            var review = new Review
            {
                ProductID = 1,
                CustomerID = 1,
                Rating = 5,
                Comment = "Excellent product!",
                Review_Date = DateTime.Now
            };
            context.Reviews.Add(review);
            await context.SaveChangesAsync();

            // Act
            review.Rating = 4;
            review.Comment = "Good product!";
            var result = await repository.Update(review);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Rating, Is.EqualTo(4));
            Assert.That(result.Comment, Is.EqualTo("Good product!"));
        }

        [Test]
        public async Task Delete_Review_ShouldDeleteReview()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new ReviewRepository(context);
            var review = new Review
            {
                ProductID = 1,
                CustomerID = 1,
                Rating = 5,
                Comment = "Excellent product!",
                Review_Date = DateTime.Now
            };
            context.Reviews.Add(review);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.Delete(review.ReviewID);

            // Assert
            Assert.NotNull(result);
            Assert.ThrowsAsync<NotFoundException>(async () => await repository.Get(review.ReviewID));
        }

        [Test]
        public async Task Get_Review_ShouldReturnReview()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new ReviewRepository(context);
            var review = new Review
            {
                ProductID = 1,
                CustomerID = 1,
                Rating = 5,
                Comment = "Excellent product!",
                Review_Date = DateTime.Now
            };
            context.Reviews.Add(review);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.Get(review.ReviewID);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Rating, Is.EqualTo(review.Rating));
        }

        [Test]
        public async Task GetAll_Review_ShouldReturnReview()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new ReviewRepository(context);
            var review = new Review
            {
                ProductID = 1,
                CustomerID = 1,
                Rating = 5,
                Comment = "Excellent product!",
                Review_Date = DateTime.Now
            };
            var review1 = new Review
            {
                ProductID = 2,
                CustomerID = 2,
                Rating = 5,
                Comment = "Excellent product!",
                Review_Date = DateTime.Now
            };
            context.Reviews.Add(review);
            context.Reviews.Add(review1);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.Get();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task Get_Review_ShouldThrowNotFoundException()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new ReviewRepository(context);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(async () => await repository.Get(999));
        }
    }
}
