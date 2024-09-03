using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models.DTO_s;
using ShoppingAppAPI.Models.DTO_s.Customer_DTO_s;
using ShoppingAppAPI.Models.DTO_s.Seller_DTO_s;
using ShoppingAppAPI.Services.Interfaces;

namespace ShoppingAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userService;
        private readonly ICustomerServices _customerService;
        private readonly ISellerServices _sellerService;
        public UserController(IUserServices userService, ICustomerServices customerService, ISellerServices sellerService)
        {
            _userService = userService;
            _customerService = customerService;
            _sellerService = sellerService;
        }

        [EnableCors("AllowSpecificOrigin")]
        [HttpPost("CustomerLogin")]
        [ProducesResponseType(typeof(LoginReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginReturnDTO>> CustomerLogin(LoginDTO userLoginDTO)
        {
            try
            {
                var result = await _userService.CustomerLogin(userLoginDTO);
                return Ok(result);
            }
            catch (UnauthorizedUserException ex)
            {
                return Unauthorized(new ErrorModel(401, ex.Message));
            }
            catch (UnableToLoginException ex)
            {
                return Unauthorized(new ErrorModel(401, ex.Message));
            }
            catch(AccountInActiveException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(500, $"An unexpected error occurred. {ex.Message}"));
            }
        }
        [EnableCors("AllowSpecificOrigin")]
        [HttpPost("CustomerRegister")]
        [ProducesResponseType(typeof(LoginReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginReturnDTO>> CustomerRegister(RegisterDTO registerDTO)
        {
            try
            {
                var result = await _userService.CustomerRegister(registerDTO);
                return Ok(result);
            }
            catch (UserAlreadyExistsException ex)
            {
                return Conflict(new ErrorModel(409, ex.Message));
            }
            catch (UnableToRegisterException ex)
            {
                return UnprocessableEntity(new ErrorModel(422, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(500, $"An unexpected error occurred. {ex.Message}"));
            }
        }
        [EnableCors("AllowSpecificOrigin")]
        [HttpPost("SellerLogin")]
        [ProducesResponseType(typeof(LoginReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginReturnDTO>> SellerLogin(LoginDTO userLoginDTO)
        {
            try
            {
                var result = await _userService.SellerLogin(userLoginDTO);
                return Ok(result);
            }
            catch (UnauthorizedUserException ex)
            {
                return Unauthorized(new ErrorModel(401, ex.Message));
            }
            catch (UnableToLoginException ex)
            {
                return Unauthorized(new ErrorModel(401, ex.Message));
            }
            catch (AccountInActiveException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(500, $"An unexpected error occurred. {ex.Message}"));
            }
        }
        [EnableCors("AllowSpecificOrigin")]
        [HttpPost("SellerRegister")]
        [ProducesResponseType(typeof(LoginReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginReturnDTO>> SellerRegister(RegisterDTO registerDTO)
        {
            try
            {
                var result = await _userService.SellerRegister(registerDTO);
                return Ok(result);
            }
            catch (UserAlreadyExistsException ex)
            {
                return Conflict(new ErrorModel(409, ex.Message));
            }
            catch (UnableToRegisterException ex)
            {
                return UnprocessableEntity(new ErrorModel(422, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(500, $"An unexpected error occurred. {ex.Message}"));
            }
        }
        [EnableCors("AllowSpecificOrigin")]
        [Authorize(Roles = "Customer")]
        [HttpPut("UpdateCustomerProfile")]
        [ProducesResponseType(typeof(CustomerDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CustomerDTO>> UpdateCustomerProfile(CustomerUpdateDTO updateDTO)
        {
            try
            {
                var result = await _customerService.UpdateCustomer(updateDTO);
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
        [EnableCors("AllowSpecificOrigin")]
        [Authorize(Roles = "Seller")]
        [HttpPut("UpdateSellerProfile")]
        [ProducesResponseType(typeof(SellerDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CustomerDTO>> UpdateSellerProfile(SellerUpdateDTO updateDTO)
        {
            try
            {
                var result = await _sellerService.UpdateSeller(updateDTO);
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

        [EnableCors("AllowSpecificOrigin")]
        [Authorize(Roles = "Customer")]
        [HttpGet("GetCustomerProfile")]
        [ProducesResponseType(typeof(CustomerDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CustomerDTO>> GetCustomerProfile(int UserID)
        {
            try
            {
                var result = await _customerService.GetCustomerProfile(UserID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(500, $"An unexpected error occurred. {ex.Message}"));
            }
        }

        [EnableCors("AllowSpecificOrigin")]
        [Authorize(Roles = "Seller")]
        [HttpGet("GetSellerProfile")]
        [ProducesResponseType(typeof(CustomerDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CustomerDTO>> GetSellerProfile(int UserID)
        {
            try
            {
                var result = await _sellerService.GetSellerProfile(UserID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(500, $"An unexpected error occurred. {ex.Message}"));
            }
        }
    }
}
