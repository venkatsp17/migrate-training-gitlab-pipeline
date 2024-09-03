using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Models.DTO_s.Review_DTO_s;
using ShoppingAppAPI.Repositories.Interfaces;
using ShoppingAppAPI.Services.Interfaces;

namespace ShoppingAppAPI.Services.Classes
{
    public class ReviewServices : IReviewServices
    {
        private readonly IRepository<int, Review> _reviewRepository;
        /// <summary>
        /// Constructor for ReviewServices class.
        /// </summary>
        /// <param name="reviewRepository">Review repository dependency.</param>
        public ReviewServices(IRepository<int, Review> reviewRepository) { 
            _reviewRepository = reviewRepository;
        }
        /// <summary>
        /// Adds a new review.
        /// </summary>
        /// <param name="reviewDto">Review information DTO.</param>
        /// <returns>Returns the added review DTO.</returns>
        /// <exception cref="UnableToAddItemException">Thrown when unable to review in the database</exception>
        public async Task<ReviewReturnDTO> AddReview(ReviewGetDTO reviewDto)
        {
            try
            {
                var review = new Review
                {
                    ProductID = reviewDto.ProductID,
                    CustomerID = reviewDto.CustomerID,
                    Rating = reviewDto.Rating,
                    Comment = reviewDto.Comment,
                    Review_Date = DateTime.Now,
                };


                var newReview = await _reviewRepository.Add(review);
                if (newReview == null)
                {
                    throw new UnableToAddItemException("Unable to add review at this moment!");
                }

                return new ReviewReturnDTO()
                {
                    ReviewID = newReview.ReviewID,
                    ProductID = newReview.ProductID,
                    CustomerID = newReview.CustomerID,
                    Rating = newReview.Rating,
                    Comment = newReview.Comment,
                    Review_Date = newReview.Review_Date,
                };
            }
            catch (Exception ex)
            {
                throw new UnableToAddItemException(ex.Message);
            }
        }
    }
}
