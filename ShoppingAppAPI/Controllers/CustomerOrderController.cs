using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models.DTO_s;
using ShoppingAppAPI.Models.DTO_s.Order_DTO_s;
using ShoppingAppAPI.Services.Classes;
using ShoppingAppAPI.Services.Interfaces;

namespace ShoppingAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerOrderController : ControllerBase
    {
        private readonly IOrderServices _orderServices;
        public CustomerOrderController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        [Authorize]
        [HttpPost("PlaceOrder")]
        [ProducesResponseType(typeof(CustomerOrderReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CustomerOrderReturnDTO>> PlaceOrder(PlaceOrderDTO placeOrderDTO)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors)
                                      .Select(e => e.ErrorMessage);
                    return BadRequest("Validation failed: " + string.Join("; ", errors));
                }
                var result = await _orderServices.PlaceOrder(placeOrderDTO);
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

        [Authorize(Roles = "Customer")]
        [HttpGet("ViewOrderHistory")]
        [ProducesResponseType(typeof(IEnumerable<CustomerOrderReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<CustomerOrderReturnDTO>>> ViewOrderHistory(int CustomerID)
        {
            try
            {
                var result = await _orderServices.ViewCustomerOrderHistory(CustomerID);
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

        [Authorize(Roles = "Customer")]
        [HttpGet("ViewCurrentOrders")]
        [ProducesResponseType(typeof(IEnumerable<CustomerOrderReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<CustomerOrderReturnDTO>>> ViewCurrentOrders(int CustomerID)
        {
            try
            {
                var result = await _orderServices.ViewAllCustomerActiveOrders(CustomerID);
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

        [Authorize(Roles = "Customer")]
        [HttpPut("CustomerCancelOrder")]
        [ProducesResponseType(typeof(IEnumerable<CustomerOrderReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<CustomerOrderReturnDTO>>> CustomerCancelOrder(int OrderID)
        {
            try
            {
                var result = await _orderServices.CustomerCancelOrder(OrderID);
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

        [Authorize(Roles = "Customer")]
        [HttpPut("UpdateOrderDeliveryDetails")]
        [ProducesResponseType(typeof(CustomerOrderReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CustomerOrderReturnDTO>> UpdateOrderDeliveryDetails(UpdateOrderDeliveryDetailsDTO deliveryDetailsDTO)
        {
            try
            {
                var result = await _orderServices.UpdateOrderDeliveryDetails(deliveryDetailsDTO);
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
    }
}
