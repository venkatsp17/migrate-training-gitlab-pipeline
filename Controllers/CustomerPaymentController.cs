using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models.DTO_s.Cart_DTO_s;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Models.DTO_s.Payment_DTO_s;
using ShoppingAppAPI.Services.Classes;
using ShoppingAppAPI.Services.Interfaces;

namespace ShoppingAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerPaymentController : ControllerBase
    {
        private readonly IPaymentServices _paymentServices;

        public CustomerPaymentController(IPaymentServices paymentServices)
        {
            _paymentServices = paymentServices;
        }

        [HttpPost("MakePayment")]
        [ProducesResponseType(typeof(PaymentReturnDTO),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel),StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PaymentReturnDTO>> MakePayment(PaymentGetDTO paymentGetDTO)
        {
            try
            {
                var result = await _paymentServices.MakePayment(paymentGetDTO);
                return Ok(result);
            }
            catch (UnableToProcessOrder ex)
            {
                return UnprocessableEntity(new ErrorModel(422, ex.Message));
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
