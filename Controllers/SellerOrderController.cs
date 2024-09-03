using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Models.DTO_s;
using ShoppingAppAPI.Models.DTO_s.Order_DTO_s;
using ShoppingAppAPI.Services.Interfaces;
using System.Security.Claims;
using static ShoppingAppAPI.Models.Enums;

namespace ShoppingAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerOrderController : ControllerBase
    {
        private readonly IOrderServices _orderServices;
        public SellerOrderController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        [Authorize(Roles = "Seller")]
        [HttpGet("ViewAllActiveOrders")]
        [ProducesResponseType(typeof(IEnumerable<SellerOrderReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<SellerOrderReturnDTO>>> ViewAllActiveOrders(int SellerID, int offset = 0, int limit = 10, string searchQuery = "")
        {
            try
            {
                var result = await _orderServices.ViewAllSellerActiveOrders(SellerID, offset, limit, searchQuery);
                return Ok(result);
            }
            catch (NoAvailableItemException ex)
            {
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(500, $"An unexpected error occurred. {ex.Message}"));
            }
        }

        [Authorize(Roles = "Seller")]
        [HttpPut("UpdateOrderStatus")]
        [ProducesResponseType(typeof(SellerOrderReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<SellerOrderReturnDTO>> UpdateOrderStatus(OrderStatus orderStatus, int OrderID)
        {
            try
            {
                var result = await _orderServices.UpdateOrderStatus(orderStatus, OrderID);
                return Ok(result);
            }
            catch (UnableToUpdateItemException ex)
            {
                return UnprocessableEntity(new ErrorModel(422, ex.Message));
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
    }
}
