using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models.DTO_s;
using ShoppingAppAPI.Services.Interfaces;

namespace ShoppingAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerProductController : ControllerBase
    {
        private readonly IProductServices _productServices;
        private readonly ICategoryServices _categoryServices;

        public CustomerProductController(IProductServices productServices, ICategoryServices categoryServices)
        {
            _productServices = productServices;
            _categoryServices = categoryServices;
        }

        [Authorize]
        [HttpGet("GetProductsByName")]
        [ProducesResponseType(typeof(IEnumerable<CustomerGetProductDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<CustomerGetProductDTO>>> GetProductsByName(string productName)
        {
            try
            {
                var result = await _productServices.GetProductsByName(productName);
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

        [Authorize]
        [HttpGet("GetProductsByCategory")]
        [ProducesResponseType(typeof(IEnumerable<CustomerGetProductDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<CustomerGetProductDTO>>> GetProductsByCategory(string categoryName)
        {
            try
            {
                var result = await _categoryServices.GetProductsByCategory(categoryName);
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

        [EnableCors("AllowSpecificOrigin")]
        [HttpGet("GetAllProducts")]
        [ProducesResponseType(typeof(IEnumerable<CustomerGetProductDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<CustomerGetProductDTO>>> GetAllProducts(int page, int pageSize, string query)
        {
            try
            {
                var result = await _productServices.GetAllProducts(page, pageSize, query);
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
    }
}
