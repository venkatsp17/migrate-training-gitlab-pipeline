using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Models.DB_Models;
using ShoppingAppAPI.Repositories.Interfaces;

namespace ShoppingAppAPI.Repositories.Classes
{
    public class ImageRepository : IImageRepository
    {
        private readonly ShoppingAppContext _context;

        public ImageRepository(ShoppingAppContext context)
        {
            _context = context;
        }

        public async Task<Image> Add(Image item)
        {
            _context.Images.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Image> Delete(int key)
        {
            var image = await _context.Images.FindAsync(key);
            if (image == null)
            {
                return null;
            }

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
            return image;
        }

        public async Task<Image> Get(int key)
        {
            return await _context.Images.FindAsync(key);
        }

        public async Task<IEnumerable<Image>> Get()
        {
            return await _context.Images.ToListAsync();
        }

        public async Task<Image> Update(Image item)
        {
            var image = await _context.Images.FindAsync(item.Id);
            if (image == null)
            {
                return null;
            }

            image.FileName = item.FileName;
            image.ContentType = item.ContentType;
            image.Data = item.Data;

            _context.Images.Update(image);
            await _context.SaveChangesAsync();
            return image;
        }
    }
}
