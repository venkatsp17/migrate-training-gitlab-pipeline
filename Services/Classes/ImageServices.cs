using ShoppingAppAPI.Models.DB_Models;
using ShoppingAppAPI.Repositories.Interfaces;
using ShoppingAppAPI.Services.Interfaces;

namespace ShoppingAppAPI.Services.Classes
{
    public class ImageServices : IImageServices
    {
        private readonly IImageRepository _imageRepository;

        public ImageServices(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }
        public async Task<Image> AddImageAsync(Image image)
        {
            try
            {
                return await _imageRepository.Add(image);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the image.", ex);
            }
        }

        public async Task<Image> DeleteImageAsync(int id)
        {
            try
            {
                var image = await _imageRepository.Get(id);
                if (image == null)
                {
                    throw new KeyNotFoundException("Image not found.");
                }
                return await _imageRepository.Delete(id);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the image.", ex);
            }
        }

        public async Task<Image> GetImageByIdAsync(int id)
        {
            try
            {
                var image = await _imageRepository.Get(id);
                if (image == null)
                {
                    throw new KeyNotFoundException("Image not found.");
                }
                return image;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching the image.", ex);
            }
        }
    }
}
