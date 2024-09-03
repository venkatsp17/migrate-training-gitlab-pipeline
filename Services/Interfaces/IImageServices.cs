using ShoppingAppAPI.Models.DB_Models;

namespace ShoppingAppAPI.Services.Interfaces
{
    public interface IImageServices
    {
        Task<Image> AddImageAsync(Image image);
        Task<Image> DeleteImageAsync(int id);
        Task<Image> GetImageByIdAsync(int id);
    }
}
