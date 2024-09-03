using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Models.DTO_s.Review_DTO_s;
using ShoppingAppAPI.Repositories.Classes;
using ShoppingAppAPI.Repositories.Interfaces;
using ShoppingAppAPI.Services.Classes;
using ShoppingAppAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingAppTest.Service_Tests
{
    [TestFixture]
    public class ReviewServicesTests
    {
        private ShoppingAppContext _context;
        private IRepository<int, Review> _reviewRepository;
        private IReviewServices _reviewServices;

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
            _reviewRepository = new ReviewRepository(_context);
            _reviewServices = new ReviewServices(_reviewRepository);

            // Seed the database with test data
            var review = new Review
            {
                ReviewID = 1,
                ProductID = 1,
                CustomerID = 1,
                Rating = 4,
                Comment = "Good product",
                Review_Date = DateTime.Now,
            };
            _context.Reviews.Add(review);
            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
        [Test]
        public async Task AddReview_ValidReview_ReturnsReviewReturnDTO()
        {
            // Arrange
            var reviewDto = new ReviewGetDTO
            {
                ProductID = 2,
                CustomerID = 2,
                Rating = 5,
                Comment = "Excellent product!"
            };

            // Act
            var result = await _reviewServices.AddReview(reviewDto);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ProductID, Is.EqualTo(reviewDto.ProductID));
            Assert.That(result.CustomerID, Is.EqualTo(reviewDto.CustomerID));
            Assert.That(result.Rating, Is.EqualTo(reviewDto.Rating));
            Assert.That(result.Comment, Is.EqualTo(reviewDto.Comment));
        }

        //[Test]
        //public void AddReview_NullReview_ThrowsUnableToAddItemException()
        //{
        //    // Arrange
        //    ReviewGetDTO reviewDto = null;

        //    // Act & Assert
        //    var ex = Assert.ThrowsAsync<UnableToAddItemException>(async () => await _reviewServices.AddReview(reviewDto));
        //    Assert.That(ex.Message, Is.EqualTo("Unable to add review at this moment: Value cannot be null. (Parameter 'reviewDto')"));
        //}

        //[Test]
        //public async Task AddReview_InvalidReview_ThrowsUnableToAddItemException()
        //{
        //    // Arrange
        //    var reviewDto = null;

        //    // Act & Assert
        //    var ex = Assert.ThrowsAsync<UnableToAddItemException>(async () => await _reviewServices.AddReview(reviewDto));
        //    Assert.That(ex.Message, Is.EqualTo("Object reference not set to an instance of an object."));
        //}
    }
}
