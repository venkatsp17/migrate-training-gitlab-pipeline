using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingAppAPI.Models.DB_Models;
using ShoppingAppAPI.Models.DTO_s.Order_DTO_s;
using ShoppingAppAPI.Services.Interfaces;

namespace ShoppingAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageAPIController : ControllerBase
    {
        private readonly IImageServices _imageService;

        public ImageAPIController(IImageServices imageService)
        {
            _imageService = imageService;
        }


        [HttpPost("upload")]
        [ProducesResponseType(typeof(JsonResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Upload([FromForm] IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest(new ErrorModel(404, "No File Sent!"));

                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);

                    var image = new Image
                    {
                        FileName = file.FileName,
                        ContentType = file.ContentType,
                        Data = memoryStream.ToArray()
                    };

                    await _imageService.AddImageAsync(image);

                    // Return the ID of the uploaded image
                    return Ok(new { Message = "Image uploaded successfully.", ImageId = image.Id });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel(404, ex.Message));
            }
        }

 
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(File), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var image = await _imageService.GetImageByIdAsync(id);

                if (image == null)
                    return NotFound(new ErrorModel(404, "Image with ID not found!"));

                return File(image.Data, image.ContentType, image.FileName);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ErrorModel(404, "Image with ID not found!"));
            }
            catch (Exception ex)
            {
                // Log the exception here if you have logging set up
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(JsonResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var image = await _imageService.DeleteImageAsync(id);

                if (image == null)
                    return NotFound(new ErrorModel(404, "Image with ID not found!"));

                return Ok(new { Message = "Image deleted successfully.", ImageId = image.Id });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ErrorModel(404, "Image with ID not found!"));
            }
            catch (Exception ex)
            {
                // Log the exception here if you have logging set up
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }
    }
}
