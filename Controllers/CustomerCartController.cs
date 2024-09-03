using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models.DTO_s.Product_DTO_s;
using ShoppingAppAPI.Models.DTO_s;
using ShoppingAppAPI.Services.Classes;
using ShoppingAppAPI.Services.Interfaces;
using ShoppingAppAPI.Models.DTO_s.Cart_DTO_s;

namespace ShoppingAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerCartController : ControllerBase
    {
        private readonly ICartServices _cartServices;

        public CustomerCartController(ICartServices cartServices)
        {
            _cartServices = cartServices;
        }

        [Authorize]
        [HttpPost("AddItemToCart")]
        [ProducesResponseType(typeof(CartReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CartReturnDTO>> AddItemToCart(CartItemGetDTO cartItemGetDTO, int CustomerID)
        {
            try
            {
                var result = await _cartServices.AddItemToCart(cartItemGetDTO,CustomerID);
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

        [Authorize]
        [HttpPut("RemoveItemFromCart")]
        [ProducesResponseType(typeof(CartReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CartReturnDTO>> RemoveItemFromCart(int CartItemID)
        {
            try
            {
                var result = await _cartServices.RemoveItemFromCart(CartItemID);
                return Ok(result);
            }
            catch (UnableToUpdateItemException ex)
            {
                return UnprocessableEntity(new ErrorModel(422, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(500, $"An unexpected error occurred. {ex.Message}"));
            }
        }

        [Authorize]
        [HttpPut("UpdateCartItemQuantity")]
        [ProducesResponseType(typeof(CartReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CartReturnDTO>> UpdateCartItemQuantity(int CartItemID, int Quantity)
        {
            try
            {
                var result = await _cartServices.UpdateCartItemQuantity(CartItemID, Quantity);
                return Ok(result);
            }
            catch (UnableToUpdateItemException ex)
            {
                return UnprocessableEntity(new ErrorModel(422, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(500, $"An unexpected error occurred. {ex.Message}"));
            }
        }

        [Authorize]
        [HttpGet("GetCart")]
        [ProducesResponseType(typeof(CartReturnDTO1), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CartReturnDTO1>> GetCart(int customerID)
        {
            try
            {
                var result = await _cartServices.GetCart(customerID);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(500, $"An unexpected error occurred. {ex.Message}"));
            }
        }

        [Authorize]
        [HttpDelete("CloseCart")]
        [ProducesResponseType(typeof(CartReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CartReturnDTO>> CloseCart(int CartID)
        {
            try
            {
                var result = await _cartServices.CloseCart(CartID);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (UnableToUpdateItemException ex)
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
