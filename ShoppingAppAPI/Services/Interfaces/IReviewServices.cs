using ShoppingAppAPI.Models;
using ShoppingAppAPI.Models.DTO_s.Review_DTO_s;

namespace ShoppingAppAPI.Services.Interfaces
{
    public interface IReviewServices
    {
        Task<ReviewReturnDTO> AddReview(ReviewGetDTO reviewDto);
    }
}
