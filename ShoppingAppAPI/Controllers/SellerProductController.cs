using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Models.DTO_s;
using ShoppingAppAPI.Models.DTO_s.Order_DTO_s;
using ShoppingAppAPI.Models.DTO_s.Product_DTO_s;
using ShoppingAppAPI.Services.Classes;
using ShoppingAppAPI.Services.Interfaces;
using static ShoppingAppAPI.Models.Enums;

namespace ShoppingAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerProductController : ControllerBase
    {
        private readonly IProductServices _productServices;

        public SellerProductController(IProductServices productServices)
        {
            _productServices = productServices;
        }

      

        [Authorize(Roles = "Seller")]
        [HttpPost("AddProduct")]
        [ProducesResponseType(typeof(SellerGetProductDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<SellerGetProductDTO>> AddProduct(AddProductDTO addProductDTO)
        {
            try
            {
                var result = await _productServices.AddProduct(addProductDTO);
                return Ok(result);
            }
            catch (ItemAlreadyExistException ex)
            {
                return Conflict(new ErrorModel(409, ex.Message));
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

        [Authorize(Roles = "Seller")]
        [HttpPut("UpdateProductPrice")]
        [ProducesResponseType(typeof(SellerGetProductDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<SellerGetProductDTO>> UpdateProductPrice(decimal NewPrice, int ProductID)
        {
            try
            {
                var result = await _productServices.UpdateProductPrice(NewPrice, ProductID);
                return Ok(result);
            }
            catch (NoAvailableItemException ex)
            {
                return NotFound(new ErrorModel(409, ex.Message));
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

        [Authorize(Roles = "Seller")]
        [HttpPut("UpdateProductStock")]
        [ProducesResponseType(typeof(SellerGetProductDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<SellerGetProductDTO>> UpdateProductStock(int stock, int ProductID)
        {
            try
            {
                var result = await _productServices.UpdateProductStock(stock, ProductID);
                return Ok(result);
            }
            catch (NoAvailableItemException ex)
            {
                return NotFound(new ErrorModel(409, ex.Message));
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

        [Authorize(Roles = "Seller")]
        [HttpGet("ViewAllProducts")]
        [ProducesResponseType(typeof(IEnumerable<SellerGetProductDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<SellerGetProductDTO>>> ViewAllProducts(int SellerID, int offset = 0, int limit = 10, string searchQuery="")
        {
            try
            {
                var result = await _productServices.ViewAllSellerProducts(SellerID, offset, limit, searchQuery);
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
        [HttpGet("ViewAllTopSellingProducts")]
        [ProducesResponseType(typeof(IEnumerable<SellerGetProductDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<SellerGetProductDTO>>> ViewAllTopSellingProducts(int SellerID)
        {
            try
            {
                var result = await _productServices.ViewAllTopSellingSellerProducts(SellerID);
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
