using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models.DTO_s.Order_DTO_s;
using ShoppingAppAPI.Models.DTO_s.Review_DTO_s;
using ShoppingAppAPI.Services.Interfaces;

namespace ShoppingAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerReviewController : ControllerBase
    {
        private readonly IReviewServices _reviewServices;

        public CustomerReviewController(IReviewServices reviewServices)
        {
            _reviewServices = reviewServices;
        }

        [Authorize]
        [HttpPost("AddReview")]
        [ProducesResponseType(typeof(ReviewReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ReviewReturnDTO>> AddReview(ReviewGetDTO reviewGetDTO)
        {

            try
            {
                //if (!ModelState.IsValid)
                //{
                //    var errors = ModelState.Values.SelectMany(v => v.Errors)
                //                      .Select(e => e.ErrorMessage);
                //    return BadRequest("Validation failed: " + string.Join("; ", errors));
                //}
                var result = await _reviewServices.AddReview(reviewGetDTO);
                return Ok(result);
            }
            catch (UnableToAddItemException ex)
            {
                return UnprocessableEntity(new ErrorModel(422, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(500, $"An unexpected error occurred. {ex.Message}"));
            }
        }
    }
}
